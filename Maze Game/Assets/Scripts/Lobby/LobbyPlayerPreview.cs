using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPreview : MonoBehaviour
{
    [SerializeField] private TeamType teamType;

    [SerializeField] private PlayerListDetails player;

    [SerializeField] private GameObject playerPreview;

    [SerializeField] private Text displayNameInput;

    public void Initialize(PlayerListDetails player)
    {
        this.player = player;

        if (player == null || player.playerExist == false)
        {
            playerPreview.SetActive(false);
        } else
        {
            playerPreview.SetActive(true);
            displayNameInput.text = player.playerName;
        }
    }
}
