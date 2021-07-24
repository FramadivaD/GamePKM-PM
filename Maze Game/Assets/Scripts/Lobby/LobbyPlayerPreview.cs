using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlayerPreview : MonoBehaviour
{
    [SerializeField] private string playerID;
    [SerializeField] private string playerName;

    [SerializeField] private InputField displayNameInput;

    public void Initialize(string playerID, string playerName)
    {
        this.playerID = playerID;
        this.playerName = playerName;

        displayNameInput.text = playerName;
    }
}
