using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour
{
    // Host only
    public static int ConnectedPlayers {  get; private set; }

    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private Button startButton;

    [SerializeField] private List<Image> playerAvatarSpritesType0;
    [SerializeField] private List<Image> playerAvatarSpritesType1;
    [SerializeField] private List<Image> playerAvatarSpritesType2;


    [SerializeField] private List<GameObject> playerBanners;
    [SerializeField] private List<TMP_Text> playerUsernamesTexts;
    [SerializeField] private List<Image> playerAvatars;

    private readonly List<ulong> playerIDs = new();
    private readonly string[] playerUsernames = new string[8];
    private readonly int[] playerAvatarTypes = new int[8];

    private readonly int numberOfAvatarTypes = 3;

    private readonly int minimumPlayers = 2;

    public override void OnNetworkSpawn()
    {
        // Set -1 as default

        for (int i = 0; i < playerAvatarTypes.Length; i++)
            playerAvatarTypes[i] = -1;

        ConnectPlayerServerRpc(NetworkManager.Singleton.LocalClientId, usernameField.text);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ConnectPlayerServerRpc(ulong clientId, string playerUsername)
    {
        // Add playerID and username
        playerIDs.Add(clientId);
        playerUsernames[playerIDs.Count - 1] = playerUsername; 

        ConnectedPlayers = playerIDs.Count;

        // Assign PlayerNumber to the connected player
        AssignPlayerNumberClientRpc(playerIDs.Count - 1, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { clientId } } });

        // Give the player a random avatar
        int newAvatarType = Random.Range(0, numberOfAvatarTypes);

        // Update playerAvatars
        playerAvatarTypes[playerIDs.Count - 1] = newAvatarType;

        // Send updated username and avatar type arrays to all clients

        //StringContainer[] playerUsernameContainers = new StringContainer[playerUsernames.Length];
        //for (int i = 0; i < playerUsernameContainers.Length; i++)
        //{
        //    playerUsernameContainers[i] = new()
        //    {
        //        containedString = playerUsernames[i]
        //    };
        //}
        UpdateAvatarsClientRpc(playerAvatarTypes, playerUsernames[0], playerUsernames[1], playerUsernames[2],
            playerUsernames[3], playerUsernames[4], playerUsernames[5], playerUsernames[6], playerUsernames[7]);



        if (playerIDs.Count < minimumPlayers)
            return;

        startButton.interactable = true;
    }

    [ClientRpc]
    private void AssignPlayerNumberClientRpc(int newPlayerNumber, ClientRpcParams clientRpcParams)
    {
        PlayerData.PlayerNumber = newPlayerNumber;
    }

    [ClientRpc]
    private void UpdateAvatarsClientRpc(int[] avatarTypes, string player1Username, string player2Username, string player3Username, 
        string player4Username, string player5Username, string player6Username, string player7Username, string player8Username)
    {
        //for (int i = 0; i < newStringContainers.Length; i++)
        //    usernames[i] = newStringContainers[i].containedString;

        playerUsernamesTexts[0].text = player1Username;
        playerUsernamesTexts[1].text = player2Username;
        playerUsernamesTexts[2].text = player3Username;
        playerUsernamesTexts[3].text = player4Username;
        playerUsernamesTexts[4].text = player5Username;
        playerUsernamesTexts[5].text = player6Username;
        playerUsernamesTexts[6].text = player7Username;
        playerUsernamesTexts[7].text = player8Username;

        for (int i = 0; i < avatarTypes.Length; i++)
        {
            // -1 is default
            if (avatarTypes[i] == -1)
                return;

            //playerAvatars[i].sprite =

            playerBanners[i].SetActive(true);
        }
    }

    public void SelectStartGame()
    {
        // Only run on Host
        StartGameClientRpc();
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        // Switch screens
    }
}