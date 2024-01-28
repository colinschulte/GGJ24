using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RelayManager : NetworkBehaviour
{
    [SerializeField] private DisplayPrompt prompt;

    [SerializeField] PlayerData player;

    [SerializeField] GameManager GM;

    // The only class that handled mid-game network logic.

    // The host player is a server AND a client.
    // This method is called by the any client. It only runs on the Host.
    // The method's name MUST end with 'ClientRpc'. RequireOwnership must be false!
    [ClientRpc]
    public void SendPlayerIDtoHostClientRpc(int playerID)
    {
        prompt.AddVote(playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void sendGradeToServerRpc(int playerNumber, int grade)
    {
        player.addGrade(playerNumber, grade);
    }

    [ClientRpc]
    public void sendMiniGameGradeClientRpc(int[] arr)
    {
        player.setArr(0, arr);
    }

    [ClientRpc]
    public void sendRankingClientRpc(int[] arr)
    {
        player.setArr(1, arr);
    }

    [ClientRpc]
    public void sendAnswerClientRpc(int playerID, string ans)
    {
        player.addAnswer(playerID, ans);
    }

    
}