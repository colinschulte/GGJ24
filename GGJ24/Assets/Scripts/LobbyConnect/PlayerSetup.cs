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

    [SerializeField] private PlayerData playerData;


    private readonly List<List<Sprite>> playerAvatarSpriteTypes = new();

    [SerializeField] private List<Sprite> playerAvatarSpritesType0;
    [SerializeField] private List<Sprite> playerAvatarSpritesType1;
    [SerializeField] private List<Sprite> playerAvatarSpritesType2;
    [SerializeField] private List<Sprite> playerAvatarSpritesType3;
    [SerializeField] private List<Sprite> playerAvatarSpritesType4;
    [SerializeField] private List<Sprite> playerAvatarSpritesType5;
    [SerializeField] private List<Sprite> playerAvatarSpritesType6;
    [SerializeField] private List<Sprite> playerAvatarSpritesType7;


    [SerializeField] private List<GameObject> playerBanners;
    [SerializeField] private List<TMP_Text> playerUsernamesTexts;
    [SerializeField] private List<Image> playerAvatars;

    private readonly List<ulong> playerIDs = new();
    private readonly int[] playerAvatarTypes = new int[8];

    private readonly int numberOfAvatarTypes = 8;

    private readonly int minimumPlayers = 1;

    private void Awake()
    {
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType0);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType1);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType2);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType3);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType4);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType5);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType6);
        playerAvatarSpriteTypes.Add(playerAvatarSpritesType7);
    }

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

        PlayerData.PlayerUsernames[playerIDs.Count - 1] = playerUsername;

        ConnectedPlayers = playerIDs.Count;

        // Assign PlayerNumber to the connected player
        AssignPlayerNumberClientRpc(playerIDs.Count - 1, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { clientId } } });

        // Give the player a random avatar
        int newRandomNumber = Random.Range(0, numberOfAvatarTypes);

        // Update playerAvatars
        playerAvatarTypes[playerIDs.Count - 1] = newRandomNumber;

        // Send updated username and avatar type arrays to all clients
        UpdateAvatarsClientRpc(playerAvatarTypes, PlayerData.PlayerUsernames[0], PlayerData.PlayerUsernames[1], PlayerData.PlayerUsernames[2],
            PlayerData.PlayerUsernames[3], PlayerData.PlayerUsernames[4], PlayerData.PlayerUsernames[5], PlayerData.PlayerUsernames[6], PlayerData.PlayerUsernames[7]);


        if (playerIDs.Count < minimumPlayers)
            return;


        startButton.interactable = true;
    }

    [ClientRpc]
    private void AssignPlayerNumberClientRpc(int newPlayerNumber, ClientRpcParams clientRpcParams)
    {
        PlayerData.PlayerNumber = newPlayerNumber;

        rpcsIn += 1;

        BothRpcsIn();
    }

    [ClientRpc]
    private void UpdateAvatarsClientRpc(int[] newAvatarTypes, string player1Username, string player2Username, string player3Username, 
        string player4Username, string player5Username, string player6Username, string player7Username, string player8Username)
    {
        avatarTypes = newAvatarTypes;

        PlayerData.PlayerUsernames[0] = player1Username;
        PlayerData.PlayerUsernames[1] = player2Username;
        PlayerData.PlayerUsernames[2] = player3Username;
        PlayerData.PlayerUsernames[3] = player4Username;
        PlayerData.PlayerUsernames[4] = player5Username;
        PlayerData.PlayerUsernames[5] = player6Username;
        PlayerData.PlayerUsernames[6] = player7Username;
        PlayerData.PlayerUsernames[7] = player8Username;

        for (int i = 0; i < avatarTypes.Length; i++)
        {
            // -1 is default
            if (avatarTypes[i] == -1)
                break;


            playerUsernamesTexts[i].text = PlayerData.PlayerUsernames[i];


            int randomNumber = avatarTypes[i];

            playerAvatars[i].sprite = playerAvatarSpriteTypes[randomNumber][i];

            playerBanners[i].SetActive(true);
        }

        rpcsIn += 1;

        BothRpcsIn();
    }

    private int rpcsIn;
    private int[] avatarTypes;
    private void BothRpcsIn()
    {
        if (rpcsIn < 2)
            return;

        int randomNumber = avatarTypes[PlayerData.PlayerNumber];

        playerData.playerSprite = playerAvatarSpriteTypes[randomNumber][PlayerData.PlayerNumber];
    }

    [SerializeField] private GameManager gameManager;
    public void SelectStartGame()
    {
        int prompt_index = Random.Range(0, PlayerData.prompts.Length);
        // Only run on Host
        StartGameClientRpc(prompt_index);
    }

    [ClientRpc]
    public void StartGameClientRpc(int p_index)
    {
        gameManager.ClientStartGame(p_index);
    }
}