using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class LobbyConnect : NetworkBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField lobbyCodeField;

    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private Button createLobbyButton;

    [SerializeField] private TMP_Text errorText;

    private Lobby currentLobby;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Username"))
            usernameField.text = PlayerPrefs.GetString("Username");
        if (PlayerPrefs.HasKey("LobbyCode"))
            lobbyCodeField.text = PlayerPrefs.GetString("LobbyCode");

        _ = ConnectToRelay();
    }

    public void OnUsernameChange()
    {
        PlayerPrefs.SetString("Username", usernameField.text);
    }
    public void OnLobbyCodeChange()
    {
        PlayerPrefs.SetString("LobbyCode", lobbyCodeField.text);
    }

    private IEnumerator ErrorMessage(string newMessage)
    {
        errorText.text = newMessage;

        yield return new WaitForSeconds(3);

        if (errorText.text == newMessage)
            errorText.text = string.Empty;
    }

    private async Task ConnectToRelay() //run in Start
    {
        errorText.text = "Connecting...";

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            StartCoroutine(ErrorMessage("Connected!"));

            ToggleEnterLobbyInterface(true);
        }
        catch
        {
            StartCoroutine(ErrorMessage("Connection failed. Please check your internet connection and restart the game"));
        }
    }

    private IEnumerator HandleLobbyHeartbeat() //keep lobby active (lobbies are automatically hidden after 30 seconds of inactivity)
    {
        while (currentLobby != null)
        {
            SendHeartbeat();
            yield return new WaitForSeconds(15);
        }
    }
    private async void SendHeartbeat()
    {
        await LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
    }


    private void ToggleEnterLobbyInterface(bool on)
    {
        createLobbyButton.interactable = on;
        joinLobbyButton.interactable = on;
        usernameField.interactable = on;
        lobbyCodeField.interactable = on;
    }


    public void SelectCreateLobby()
    {
        if (UsernameLobbyError())
            return;

        CreateLobby();
    }

    public void SelectJoinLobby()
    {
        if (UsernameLobbyError())
            return;

        JoinLobby();
    }

    private bool UsernameLobbyError()
    {
        if (usernameField.text == string.Empty)
        {
            StartCoroutine(ErrorMessage("Must choose a username!"));
            return true;
        }

        if (lobbyCodeField.text == string.Empty)
        {
            StartCoroutine(ErrorMessage("Must provide a lobby code!"));
            return true;
        }

        return false;
    }


    public async void CreateLobby()
    {
        try
        {
            // Check for existing lobbies with the provided code

#pragma warning disable IDE0017 // Simplify object initialization
            QueryLobbiesOptions queryLobbiesOptions = new()
            {
                Count = 50
            };
#pragma warning restore IDE0017 // Simplify object initialization
            queryLobbiesOptions.Filters = new List<QueryFilter>()
            {
                new(field: QueryFilter.FieldOptions.S2, op: QueryFilter.OpOptions.EQ, value: lobbyCodeField.text)
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            if (queryResponse.Results.Count != 0)
            {
                StartCoroutine(ErrorMessage("A lobby with that code already exists. Please choose another code"));
                return;
            }

                // Create Lobby

            // Lobby is public by default
            currentLobby = await LobbyService.Instance.CreateLobbyAsync("NewLobby", 8); // Number of players

            Debug.Log("Created Lobby");

            StartCoroutine(HandleLobbyHeartbeat());

            Allocation hostAllocation = await RelayService.Instance.CreateAllocationAsync(7); // Number of non-host connections
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(hostAllocation, "dtls"));

            NetworkManager.Singleton.StartHost();

            // Set up JoinAllocation
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocation.AllocationId);

            // SaveJoinCodeInLobbyData
            try
            {
                // Update currentLobby
                currentLobby = await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject> //JoinCode = S1
                    {
                        // JoinCode is never displayed to the player--it's automatically generated by the server and used behind the scenes
                        // LobbyCode is the code provided by the player

                        { "JoinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode, DataObject.IndexOptions.S1) },
                        { "LobbyCode", new DataObject(DataObject.VisibilityOptions.Public, lobbyCodeField.text, DataObject.IndexOptions.S2) }
                    }
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobby()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new()
            {
                Count = 50
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
      
            if (queryResponse.Results.Count == 0)
            {
                Debug.Log("No lobby found");
                return;
            }
      
            if (queryResponse.Results[0].AvailableSlots == 0)
            {
                Debug.Log("Lobby is already full");
                return;
            }
      
            if (queryResponse.Results[0].Data == null || !queryResponse.Results[0].Data.ContainsKey("JoinCode"))
            {
                // Data is null when no data values exist, such as a JoinCode
                // JoinCode is created when host is first connected to relay. It's possible to join the lobby before the relay connection
                // is established and before JoinCode is created
                Debug.Log("Lobby is still being created, trying again in 2 seconds");
                Invoke(nameof(JoinLobby), 2);
                return;
            }
      
            currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
      
            Debug.Log("Joined Lobby");
      
            string joinCode = currentLobby.Data["JoinCode"].Value;
      
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
      
            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public override void OnNetworkSpawn()
    {
        StartCoroutine(ErrorMessage("Joined Lobby!"));

        ToggleEnterLobbyInterface(false);
    }

    //public override void OnNetworkSpawn()
    //{
    //    ListenForClientDisconnect();

    //    if (IsServer)
    //        NetworkManager.Singleton.SceneManager.LoadScene(gameScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    //}

    //public void BackToMainMenu() //called by BackToMainMenu
    //{
    //    LeaveLobby();
    //    StartCoroutine(WaitForShutdown());
    //}
    //private IEnumerator WaitForShutdown()
    //{
    //    //delaying the scene change gives time for the shutdown to occur. This is an editor-only solution
    //    //for a known Netcode for Gameobjects bug in which the scene changing conflicts with the recent shutdown
    //    //to cause errors, despite NetworkManager.ShutdownInProgress returning false at the time. In builds, the
    //    //shutdown will always fully complete and the scenes will always fully unload/load
    //    //In short, without the delay, a bug causes an error which disrupts playmode in the editor despite nothing else being wrong
    //    if (Application.isEditor)
    //        yield return new WaitForSeconds(1);
    //    SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    //}

    public void ExitGame()
    {
        LeaveLobby();

        Application.Quit();
    }

    private async void LeaveLobby()
    {
        try
        {
            if (currentLobby != null)
            {
                if (IsServer)
                    await Lobbies.Instance.DeleteLobbyAsync(currentLobby.Id);
                else
                    await Lobbies.Instance.RemovePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId);
            }
            
            // Avoids heartbeat errors in editor since playmode doesn't stop
            currentLobby = null;

            NetworkManager.Singleton.Shutdown();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //public delegate void EnemyDisconnectedAction();
    //public static event EnemyDisconnectedAction EnemyDisconnected;

    //private void ListenForClientDisconnect() //called in OnNetworkSpawn
    //{
    //    NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    //}
    //public override void OnNetworkDespawn()
    //{
    //    NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    //}

    //private void OnClientDisconnect(ulong clientId)
    //{
    //    //if is host and is shutting down, OnClientDisconnect will be called for each connected
    //    //client that is being forcibly disconnected
    //    if (NetworkManager.Singleton.ShutdownInProgress) return;

    //    if (clientId != NetworkManager.Singleton.LocalClientId)
    //        EnemyDisconnected?.Invoke();
    //}
}