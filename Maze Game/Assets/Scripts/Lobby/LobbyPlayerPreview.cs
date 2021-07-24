using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPreview : MonoBehaviour
{
    [SerializeField] private TeamType teamType;

    [SerializeField] private PhotonPlayer player;

    [SerializeField] private GameObject playerPreview;

    [SerializeField] private Text displayNameInput;

    public void Initialize(PhotonPlayer player)
    {
        this.player = player;

        if (player == null)
        {
            playerPreview.SetActive(false);
        } else
        {
            playerPreview.SetActive(true);
            displayNameInput.text = player.NickName;
        }
    }
}
