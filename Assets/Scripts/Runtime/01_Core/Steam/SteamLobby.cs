using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private const string HostAddressKey = "HostAddress";

    private void Start()
    {
        if (!SteamManager.Initialized) return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // [호스트] 방 만들기 버튼 등에 연결
    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
    }

    // 로비가 성공적으로 생성되었을 때 실행
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        // Mirror 호스트 시작
        CustomNetworkManager.Instance.StartHost();

        // 내 Steam ID를 로비 데이터에 저장 (접속 주소 역할)
        CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(lobbyId, HostAddressKey, SteamUser.GetSteamID().ToString());
        
        Debug.Log("로비 생성 성공! 이제 친구를 초대할 수 있습니다.");
    }

    // [게스트] 스팀 친구 목록에서 '참여'를 누르거나 초대를 수락했을 때
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    // 로비에 입장했을 때 실제 서버 주소로 접속 시도
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) return; // 이미 호스트라면 무시

        CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        string hostAddress = SteamMatchmaking.GetLobbyData(lobbyId, HostAddressKey);

        CustomNetworkManager.Instance.networkAddress = hostAddress;
        CustomNetworkManager.Instance.StartClient();
    }
}
