using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public void GotoMainMenu()
    {
        Debug.Log("Go to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void GotoLobbyMenu()
    {
        Debug.Log("Go to Lobby");
        SceneManager.LoadScene("LobbyMenu");
    }

    public void GotoCreateQuestion()
    {
        Debug.Log("Go to Create Question Scene");
        SceneManager.LoadScene("CreateQuestionScene");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
