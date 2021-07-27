using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private LobbyNetwork lobbyNetwork;

    [SerializeField] private InputField createGameInput;
    [SerializeField] private InputField joinGameInput;
    [SerializeField] private InputField usernameInput;

    [SerializeField] private Text roomIDText;

    private void Start()
    {
        createGameInput.onValidateInput += CapslockInputField;
        joinGameInput.onValidateInput += CapslockInputField;
    }

    private char CapslockInputField(string str, int i, char c)
    {
        return char.ToUpper(c);
    }

    public void CreateNewGame()
    {
        string roomID = createGameInput.text;
        if (roomID.Length == 5)
        {
            Debug.Log("Creating New Game on : " + roomID);

            lobbyNetwork.CreateNewGame(roomID);
            roomIDText.text = "ROOM ID : " + roomID;
        } else
        {
            Debug.Log("Invalid ID. Must 5 characters.");
        }
    }

    public void JoinGame()
    {
        string roomID = joinGameInput.text;
        if (roomID.Length == 5)
        {
            Debug.Log("Joining Game on : " + roomID);

            lobbyNetwork.JoinGame(roomID);
            roomIDText.text = "ROOM ID : " + roomID;
        }
        else
        {
            Debug.Log("Invalid ID. Must 5 characters.");
        }
    }

    public void JoinLobbyGame()
    {
        string username = usernameInput.text;
        if (username.Length >= 3)
        {
            Debug.Log("Joining Game on : " + username);

            lobbyNetwork.JoinLobbyGame(username);
        }
        else
        {
            Debug.Log("Invalid Username. Must at least 3 characters.");
        }
    }
}
