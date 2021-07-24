using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPreview : MonoBehaviour
{
    [SerializeField] private TeamType teamType;

    [SerializeField] private string playerID;
    [SerializeField] private string playerName;

    [SerializeField] private GameObject playerPreview;

    [SerializeField] private Text displayNameInput;

    public void Initialize(string playerID, string playerName)
    {        
        this.playerID = playerID;
        this.playerName = playerName;

        if (playerID == null || playerName == null)
        {
            playerPreview.SetActive(false);
        } else
        {
            playerPreview.SetActive(true);
            displayNameInput.text = playerName;
        }
    }
}
