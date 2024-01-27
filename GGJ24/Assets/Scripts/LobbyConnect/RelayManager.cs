using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RelayManager : NetworkBehaviour
{
    // The only class that handled mid-game network logic.

    // The host player is a server AND a client.
    // This method is called by the any client. It only runs on the Host.
    // The method's name MUST end with 'ServerRpc'. RequireOwnership must be false!
    [ServerRpc (RequireOwnership = false)]
    public void ExampleServerRpc(int exampleInt)
    {
        ExampleClientRpc(exampleInt);
    }

    // This method is called by the server (host) and run on ALL clients
    // This method's name MUST end with 'ClientRpc'
    [ClientRpc]
    public void ExampleClientRpc(int exampleInt)
    {

    }


}