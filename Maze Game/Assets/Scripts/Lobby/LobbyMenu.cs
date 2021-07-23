using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyMenu : MonoBehaviour
{
    public InputField createGameInput;
    public InputField joinGameInput;

    public void CreateNewGame()
    {
        string roomID = createGameInput.text;
        if (roomID.Length == 5)
        {
            Debug.Log("Creating New Game on : " + roomID);
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
        } else
        {
            Debug.Log("Invalid ID. Must 5 characters.");
        }
    }
}
