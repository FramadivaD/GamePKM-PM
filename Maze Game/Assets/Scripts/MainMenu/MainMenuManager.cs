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

    public void GotoLaboratory()
    {
        Debug.Log("Go to Laboratory Scene");
        SceneManager.LoadScene("Laboratory");
    }

    public void GotoGameplay()
    {
        Debug.Log("Go to Gameplay Scene");

        // TODO : Rename SampleScene ke Gameplay
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
