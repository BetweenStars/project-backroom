using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Text[] playerNameTexts = new Text[4];
    [SerializeField] private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            CustomNetworkManager.Instance.ServerChangeScene("GameScene");
        });
    }

    private void Update()
    {
        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            if (i < CustomNetworkManager.Instance.players.Count)
            {
                PlayerObjectController player = CustomNetworkManager.Instance.players[i];
                playerNameTexts[i].text = player.playerName;
            }
            else
            {
                playerNameTexts[i].text = "빈 자리";
            }
        }
    }
}
