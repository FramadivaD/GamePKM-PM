using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Extensione.Window;

public class LobbyNetwork : Photon.PunBehaviour
{
    private string GameVersion { get { return Application.version; } }

    [SerializeField] LobbyPlayerList lobbyPlayerList;

    [SerializeField] private GameObject joinLobbyMenu;
    [SerializeField] private GameObject usernameLobbyMenu;
    [SerializeField] private GameObject playerLobbyMenu;

    [SerializeField] private InputField usernameInputField;

    [SerializeField] private GameObject teacherDashboardMenu;
    [SerializeField] private GameObject studentDashboardMenu;

    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;

    private bool connected = false;

    private void Awake()
    {
        OpenJoinLobby();

        createGameButton.interactable = false;
        joinGameButton.interactable = false;

        InitializePhotonNetwork();
    }

    private void InitializePhotonNetwork()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                PhotonNetwork.room.IsOpen = true;
                PhotonNetwork.room.IsVisible = true;
            }
        }

        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

    public void CreateNewGame(string roomID)
    {
        if (connected)
        {
            if (roomID.Length == 5)
            {
                PhotonNetwork.CreateRoom(roomID, new RoomOptions() { MaxPlayers = 9, PublishUserId = true }, null);
            } else
            {
                Debug.Log("RoomID must 5 characters.");

                WindowMaster.Instance.Show("RoomID harus terdiri atas 5 karakter!");
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

                WindowMaster.Instance.Show("RoomID harus terdiri atas 5 karakter!");
            }
        }
    }

    public void JoinLobbyGame(string username)
    {
        if (connected)
        {
            PhotonNetwork.playerName = username;

            teacherDashboardMenu.SetActive(PhotonNetwork.player.IsMasterClient);
            studentDashboardMenu.SetActive(!PhotonNetwork.player.IsMasterClient);

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

        createGameButton.interactable = true;
        joinGameButton.interactable = true;

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

        createGameButton.interactable = false;
        joinGameButton.interactable = false;
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Create or Join Room Failed");

        Debug.Log("Room might full or ingame progress");

        WindowMaster.Instance.Show("Join Room Gagal!\nRoom telah penuh atau sedang dimulai!");
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
