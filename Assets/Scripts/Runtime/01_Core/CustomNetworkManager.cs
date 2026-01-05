using Mirror;
using Steamworks;
using UnityEngine;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager Instance { get; private set; }

    public List<PlayerObjectController> players { get; } = new List<PlayerObjectController>();

    public override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        PlayerObjectController player = conn.identity.GetComponent<PlayerObjectController>();

        if (player != null && !players.Contains(player))
        {
            players.Add(player);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {

            PlayerObjectController player = conn.identity.GetComponent<PlayerObjectController>();

            if (player != null)
            {
                players.Remove(player);
            }
        }

        base.OnServerDisconnect(conn);
    }

    public void RegisterHostPlayer(PlayerObjectController hostPlayer)
    {
        if (hostPlayer != null && !players.Contains(hostPlayer))
        {
            players.Add(hostPlayer);
        }
    }
}