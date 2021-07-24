using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyNetwork : MonoBehaviour
{
    private string GameVersion { get { return Application.version; } }

    [SerializeField] private GameObject joinLobbyMenu;
    [SerializeField] private GameObject usernameLobbyMenu;
    [SerializeField] private GameObject playerLobbyMenu;

    [SerializeField] private InputField usernameInputField;

    private bool connected = false;

    private void Awake()
    {
        OpenJoinLobby();

        InitializePhotonNetwork();
    }

    private void InitializePhotonNetwork()
    {
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

    private void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");

        connected = true;
    }

    public void CreateNewGame(string roomID)
    {
        if (connected)
        {
            if (roomID.Length == 5)
            {
                OpenUsernameLobby();
            } else
            {
                Debug.Log("RoomID must 5 characters.");
            }
        }
    }

    public void JoinGame(string roomID)
    {
        if (connected)
        {
            if (roomID.Length == 5)
            {
                OpenUsernameLobby();
            }
            else
            {
                Debug.Log("RoomID must 5 characters.");
            }
        }
    }

    public void JoinLobbyGame(string username)
    {
        if (connected)
        {
            OpenPlayerLobby();
        }
    }

    public void LeaveLobbyGame()
    {
        if (connected)
        {
            OpenJoinLobby();
        }
    }

    public void OpenJoinLobby()
    {
        joinLobbyMenu.SetActive(true);
        playerLobbyMenu.SetActive(false);
        usernameLobbyMenu.SetActive(false);
    }

    public void OpenPlayerLobby()
    {
        joinLobbyMenu.SetActive(false);
        playerLobbyMenu.SetActive(true);
        usernameLobbyMenu.SetActive(false);
    }

    public void OpenUsernameLobby()
    {
        joinLobbyMenu.SetActive(false);
        playerLobbyMenu.SetActive(false);
        usernameLobbyMenu.SetActive(true);
    }
}
