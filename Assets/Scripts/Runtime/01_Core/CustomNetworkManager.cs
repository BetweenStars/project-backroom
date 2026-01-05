using System.Collections.Generic;
using Mirror;
using NUnit.Framework.Constraints;
using Steamworks;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static NetworkManager Instance { get; private set; }

    private PlayerObjectController localPlayer;
    public List<PlayerObjectController> players{get;} = new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }
}
