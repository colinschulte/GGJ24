using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    public static int PlayerNumber {  get; private set; }

    [SerializeField] private TMP_InputField usernameField;

    private readonly List<ulong> playerIDs = new();
    private readonly List<string> playerUsernames = new();

    private readonly int minimumPlayers = 2;

    public override void OnNetworkSpawn()
    {
        ConnectPlayerServerRpc(NetworkManager.Singleton.LocalClientId, usernameField.text);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ConnectPlayerServerRpc(ulong clientId, string playerUsername)
    {
        // Wait until all players have loaded into the scene
        playerIDs.Add(clientId);
        playerUsernames.Add(playerUsername);

        // Assign PlayerNumber to the connected player
        AssignPlayerNumberClientRpc(playerIDs.Count - 1, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { clientId } } });

        // Display connected players

        if (playerIDs.Count < minimumPlayers)
            return;

        // Allow host to start game if enough players are ready
    }

    [ClientRpc]
    private void AssignPlayerNumberClientRpc(int newPlayerNumber, ClientRpcParams clientRpcParams)
    {
        PlayerNumber = newPlayerNumber;
        Debug.Log(PlayerNumber);
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        // Called by the Host, run on all Clients

        // Start game
    }
}