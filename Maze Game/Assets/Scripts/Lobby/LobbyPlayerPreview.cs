using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPreview : MonoBehaviour
{
    [SerializeField] private TeamType teamType;

    [SerializeField] private string playerName;

    [SerializeField] private GameObject playerPreview;

    [SerializeField] private Text displayNameInput;

    public void Initialize(string playerName)
    {        
        this.playerName = playerName;

        if (playerName == null || playerName.Length == 0)
        {
            playerPreview.SetActive(false);
        } else
        {
            playerPreview.SetActive(true);
            displayNameInput.text = playerName;
        }
    }
}
