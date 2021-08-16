using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Extensione.Window;

public class LobbyNetwork : Photon.PunBehaviour
{
    private string GameVersion { get { return Application.version; } }

    [SerializeField] PhotonView pv;

    [SerializeField] LobbyPlayerList lobbyPlayerList;

    [SerializeField] private GameObject joinLobbyMenu;
    [SerializeField] private GameObject usernameLobbyMenu;
    [SerializeField] private GameObject playerLobbyMenu;

    [SerializeField] private InputField usernameInputField;

    [SerializeField] private GameObject teacherDashboardMenu;
    [SerializeField] private GameObject studentDashboardMenu;

    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;
    [SerializeField] private Button exitGameButton;

    private bool connected = false;

    private void Awake()
    {
        OpenJoinLobby();

        if (PhotonNetwork.connected)
        {
            createGameButton.interactable = true;
            joinGameButton.interactable = true;

            connected = true;
        }
        else
        {
            createGameButton.interactable = false;
            joinGameButton.interactable = false;
        }

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

        exitGameButton.interactable = true;
    }

    public void CreateNewGame(string roomID)
    {
        if (connected)
        {
            if (roomID.Length > 0)
            {
                PhotonNetwork.CreateRoom(roomID, new RoomOptions() { MaxPlayers = 9, PublishUserId = true }, null);

                exitGameButton.interactable = false;
            } else
            {
                Debug.Log("RoomID must have a character.");

                WindowMaster.Instance.Show("RoomID must have at least 1 character!");
            }
        }
    }

    public void JoinGame(string roomID)
    {
        if (connected)
        {
            if (roomID.Length > 0)
            {
                PhotonNetwork.JoinRoom(roomID);

                exitGameButton.interactable = false;
            }
            else
            {
                Debug.Log("RoomID must have a character.");

                WindowMaster.Instance.Show("RoomID must have at least 1 character!");
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

            exitGameButton.interactable = false;
        }
    }

    public void LeaveLobbyGame()
    {
        if (connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                pv.RPC("LeaveLobbyGameForcedRPC", PhotonTargets.AllBuffered);
            } else
            {
                LeaveLobbyGameRPC();
            }
        }
    }

    private void LeaveLobbyGameRPC()
    {
        PhotonNetwork.LeaveRoom();

        exitGameButton.interactable = true;
    }

    [PunRPC]
    private void LeaveLobbyGameForcedRPC()
    {
        if (!PhotonNetwork.player.IsMasterClient)
        {
            WindowMaster.Instance.Show("Master leave the room. Game Aborted.");
        }

        PhotonNetwork.LeaveRoom();

        exitGameButton.interactable = true;
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
            // if client retrieve master team data
            if (!PhotonNetwork.player.IsMasterClient)
            {
                lobbyPlayerList.RetrievePlayerTeamType();
            }
            OpenUsernameLobby();
        }

        exitGameButton.interactable = true;
    }

    public override void OnLeftRoom()
    {
        OpenJoinLobby();

        createGameButton.interactable = false;
        joinGameButton.interactable = false;

        exitGameButton.interactable = true;
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Join Room Failed");

        Debug.Log("Room might full or ingame progress");

        WindowMaster.Instance.Show("Join Room Failed!\nThe Room is full or in progress!");

        exitGameButton.interactable = true;
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Create Room Failed");

        Debug.Log("Room might exists or ingame progress");

        WindowMaster.Instance.Show("Create Room Failed!\nRoom exist, please join the room!");

        exitGameButton.interactable = true;
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
