using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPreview : MonoBehaviour
{
    [SerializeField] private TeamType teamType;

    [SerializeField] private PlayerListDetails player;

    [SerializeField] private GameObject playerPreview;

    [SerializeField] private Image playerPreviewHat;
    [SerializeField] private Image playerPreviewBody;

    [SerializeField] private Text displayNameInput;

    [SerializeField] private Sprite[] playersHatColorType;
    [SerializeField] private Sprite[] playersBodyColorType;

    public void Initialize(PlayerListDetails player, int colorType)
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

        playerPreviewHat.sprite = playersHatColorType[colorType];
        playerPreviewBody.sprite = playersBodyColorType[colorType];
    }
}
