using Mirror;
using UnityEngine;

public class PlayerObjectController : NetworkBehaviour {
    [SerializeField] private GameObject playerModel;

    [SyncVar(hook = nameof(HandleNameChange))] public string playerName;
    [SyncVar] public ulong steamID;

    public override void OnStartAuthority()
    {
        string playerName = Steamworks.SteamFriends.GetPersonaName();
        ulong steamID = Steamworks.SteamUser.GetSteamID().m_SteamID;

        CmdSetPlayerData(playerName, steamID);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        if (isServer && isLocalPlayer)
        {
            CustomNetworkManager.Instance.RegisterHostPlayer(this);
        }
        
        if (playerModel != null)
        {
            playerModel.SetActive(true);
        }
    }

    [Command]
    private void CmdSetPlayerData(string playerName, ulong steamID)
    {
        this.playerName = playerName;
        this.steamID = steamID;
    }
    
    private void HandleNameChange(string oldValue, string newValue)
    {
        
    }
}