using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyNetwork : Photon.PunBehaviour
{
    private string GameVersion { get { return Application.version; } }

    [SerializeField] LobbyPlayerList lobbyPlayerList;

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

    public void CreateNewGame(string roomID)
    {
        if (connected)
        {
            if (roomID.Length == 5)
            {
                PhotonNetwork.CreateRoom(roomID, new RoomOptions() { MaxPlayers = 9 }, null);
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
                PhotonNetwork.JoinRoom(roomID);
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
            PhotonNetwork.playerName = username;
            
            lobbyPlayerList.Initialize();
            OpenPlayerLobby();
        }
    }

    public void LeaveLobbyGame()
    {
        if (connected)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");

        connected = true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Create or Join Room Success");

        if (PhotonNetwork.player.IsMasterClient)
        {
            JoinLobbyGame("MASTER");
        } else
        {
            OpenUsernameLobby();
        }
    }

    public override void OnLeftRoom()
    {
        OpenJoinLobby();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Create or Join Room Failed");
    }

    #endregion

    #region About Window Menu

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

    #endregion
}
