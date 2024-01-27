using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RelayManager : NetworkBehaviour
{
    [SerializeField] private DisplayPrompt prompt;

    // The only class that handled mid-game network logic.

    // The host player is a server AND a client.
    // This method is called by the any client. It only runs on the Host.
    // The method's name MUST end with 'ClientRpc'. RequireOwnership must be false!
    [ClientRpc]
    public void SendPlayerIDtoHostClientRpc(int playerID)
    {
        prompt.TallyVotes(playerID);
    }


}