using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

using Extensione.Window;

public class MultiplayerNetworkMaster : Photon.PunBehaviour
{
    public static MultiplayerNetworkMaster Instance { get; private set; }
    public NetworkUIManager networkUIManager;
    public ScoreManager scoreManager;
    public RoomTeamChat roomTeamChat;

    public Camera masterCamera;

    public PhotonView pv;

    public QuestionDifficulty questionDifficulty;
    public MainGateKeyRaw mainGateRaw;

    [SerializeField] private FirebaseManager firebaseManager;

    private int playerSceneCount = 0;
    private int playerReadyCount = 0;
    private bool masterClientInitialized = false;

    public bool testClientSingle;

    [SerializeField] private GameObject masterStopGameButton;
    [SerializeField] private GameObject masterExitGameButton;

    private void Awake()
    {
        Instance = this;
    }

    private void TestClientSingle()
    {
        if (testClientSingle)
        {
            PhotonNetwork.player.NickName = "Anak Pintar";
            PhotonNetwork.ConnectUsingSettings(Application.version);
        }
    }

    public override void OnConnectedToMaster()
    {
        if (testClientSingle)
        {
            PhotonNetwork.CreateRoom("aaaaa");
        }
    }

    public override void OnJoinedRoom()
    {
        if (testClientSingle)
        {
            //string filename = "D:/Music/MathGame/Data/JPG Test.soal";
            string filename = "D:/Music/MathGame/Data/pake ini.soal";

            string fileBaseName = System.IO.Path.GetFileNameWithoutExtension(new System.IO.FileInfo(filename).Name);

            byte[] data = System.IO.File.ReadAllBytes(filename);

            string content = System.Text.Encoding.ASCII.GetString(data);

            MainGateKeyRaw testKey = JsonUtility.FromJson<MainGateKeyRaw>(content);

            LobbyTeacherRoomMainGate.CurrentMainGateKey = testKey;

            LoadMainGateKeyAndQuestionDifficultyData(LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty, LobbyTeacherRoomMainGate.CurrentMainGateKey);

            if (PhotonNetwork.player.IsMasterClient)
            {
                Debug.Log("Karena master maka dia yang generate level, biar client bisa menyesuaikan.");
                GameManager.Instance.GenerateLevel();

                Debug.Log("Karena client maka akan main sebagai player.");
                PlayAsPlayer();
            }
        }
    }

    private void Start()
    {
        WindowMaster.Instance.Hide();
        
        if (!testClientSingle)
        {
            GameManager.Instance.DisableAllEntitiesMovement("nothing");

            if (!PhotonNetwork.player.IsMasterClient)
            {
                networkUIManager.StartAsClient();

                pv.RPC("AddClientPlayer", PhotonTargets.MasterClient);
            }
            else
            {
                networkUIManager.StartAsMaster();

                Debug.Log("Waiting Player Count : " + playerSceneCount + "/" + PhotonNetwork.playerList.Length);
                AddClientPlayer();

                StartCoroutine(WaitingPlayerCountUntilTimeout(20));
            }
        } else
        {
            Invoke("TestClientSingle", 1);
        }
    }

    private IEnumerator WaitingPlayerCountUntilTimeout(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        Debug.Log("Player Count Wait Timeout, Skip to Initialize Master Client.");

        InitializeMasterClient();
    }

    [PunRPC]
    private void AddClientPlayer()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            playerSceneCount++;

            Debug.Log("A Player Joined Scene.");
            Debug.Log("Waiting Player Count : " + playerSceneCount + "/" + PhotonNetwork.playerList.Length);

            networkUIManager.MasterCountPlayer(playerSceneCount, PhotonNetwork.playerList.Length);

            if (playerSceneCount >= PhotonNetwork.playerList.Length)
            {
                StopAllCoroutines();

                Debug.Log("Player Count Satisfied, Initialize Master Client.");

                InitializeMasterClient();
            }
        }
    }

    private void InitializeMasterClient()
    {
        InitializeMasterClientTeam();
        InitializeMasterClientData();
    }

    private void InitializeMasterClientTeam()
    {
        if (PhotonNetwork.connected) {
            if (PhotonNetwork.player.IsMasterClient)
            {
                pv.RPC("ReceiveTeamColorDataClient",
                    PhotonTargets.AllBuffered,
                    LobbyPlayerList.PlayerRedTeamColorIndex,
                    LobbyPlayerList.PlayerBlueTeamColorIndex);
            }
        }
    }

    [PunRPC]
    private void ReceiveTeamColorDataClient(int redColor, int blueColor)
    {
        LobbyPlayerList.PlayerRedTeamColorIndex = redColor;
        LobbyPlayerList.PlayerBlueTeamColorIndex = blueColor;
    }

    private void InitializeMasterClientData()
    {
        string diffJson = LobbyTeacherRoomQuestionDifficulty.SelectedDifficultyJson;
        string keyJsonDownloadURL = LobbyTeacherRoom.MainGateDownloadURL;

        Debug.Log("Karena master jadi akan ngirim data");

        networkUIManager.MasterSendMainGateData();

        pv.RPC("ReceiveMainGateKeyDownloadURLAndQuestionDifficultyData", PhotonTargets.AllBuffered, diffJson, keyJsonDownloadURL);
    }

    [PunRPC]
    private void ReceiveMainGateKeyDownloadURLAndQuestionDifficultyData(string diffJson, string keyJsonDownloadURL)
    {
        networkUIManager.DownloadMainGateData();

        firebaseManager.DownloadData(keyJsonDownloadURL, 
            (byte[] data) => {
                string keyJson = System.Text.Encoding.ASCII.GetString(data);

                Debug.Log("Download MainGateKey Data Success!");

                networkUIManager.DownloadMainGateDataSuccess();

                Debug.Log("Download URL : " + keyJsonDownloadURL);

                ReceiveMainGateKeyAndQuestionDifficultyData(diffJson, keyJson);
            }, 
            () => {
                Debug.Log("Download MainGateKey Data Failed.. Sadge.");

                networkUIManager.DownloadMainGateDataFailed();

                Debug.Log("Download URL : " + keyJsonDownloadURL);

                Invoke("BackToLobby", 3);
            }
        );
    }

    private void BackToLobby()
    {
        GameManager.Instance.BackToLobby();
    }

    private void ReceiveMainGateKeyAndQuestionDifficultyData(string diffJson, string keyJson)
    {
        Debug.Log("Meskipun master / client akan menerima data.");

        questionDifficulty = JsonUtility.FromJson<QuestionDifficulty>(diffJson);
        mainGateRaw = JsonUtility.FromJson<MainGateKeyRaw>(keyJson);

        LoadMainGateKeyAndQuestionDifficultyData(questionDifficulty, mainGateRaw);

        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("Sabar, nunggu download semua selesai");

            TellMasterThatClientReadyToPlay();

            StartCoroutine(WaitingPlayerReadyUntilTimeout(20));
        }
        else
        {
            Debug.Log("Karena client maka akan main sebagai player.");

            PlayAsPlayer();

            Debug.Log("Sabar, nunggu client selesai download semua");

            pv.RPC("TellMasterThatClientReadyToPlay", PhotonTargets.MasterClient);
        }
    }

    private IEnumerator WaitingPlayerReadyUntilTimeout(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                Debug.Log("Player Ready Wait Timeout, Skip to Prepare Game as Master.");

                PrepareGameAsMaster();
            }
        } else
        {
            Debug.Log("Player Ready Wait Timeout, Skip to Prepare Game as Master.");

            PrepareGameAsMaster();
        }
    }

    [PunRPC]
    private void TellMasterThatClientReadyToPlay()
    {

        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("Cuman berjalan di Master, bakal nunggu sampai semua client siap generate level");

            Debug.Log("Waiting Player Count : " + playerReadyCount + "/" + PhotonNetwork.playerList.Length);

            networkUIManager.MasterCountPlayer(playerReadyCount, PhotonNetwork.playerList.Length);

            playerReadyCount++;

            if (playerReadyCount >= PhotonNetwork.playerList.Length)
            {
                StopAllCoroutines();

                PrepareGameAsMaster();
            }
        }
    }

    private void PrepareGameAsMaster()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                PrepareGameAsMasterExecute();
            }
        } else
        {
            PrepareGameAsMasterExecute();
        }
    }

    bool isLevelGenerated = false;

    private void PrepareGameAsMasterExecute()
    {
        if (!isLevelGenerated)
        {
            isLevelGenerated = true;

            networkUIManager.MasterShowStartGameButton();
            networkUIManager.MasterWaitToStartTheGame();

            Debug.Log("Kalau player udah siap semua maka lanjut generate level dan main sebagai spectator");

            Debug.Log("Karena master maka dia yang generate level, biar client bisa menyesuaikan.");
            GameManager.Instance.GenerateLevel();
        }
    }

    public void StartGameAsMaster()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                StartGameAsMasterAfterMasterChecking();
            }
        }
        else
        {
            StartGameAsMasterAfterMasterChecking();
        }
    }

    private void StartGameAsMasterAfterMasterChecking()
    {
        Debug.Log("Karena master maka akan main sebagai spectator.");

        pv.RPC("EnableAllEntities", PhotonTargets.AllBuffered);
        pv.RPC("FindAllChestsRPC", PhotonTargets.AllBuffered);

        PlayAsSpectator();
    }

    [PunRPC]
    private void EnableAllEntities()
    {
        GameManager.Instance.EnableAllEntitiesMovement("nothing");
    }

    [PunRPC]
    private void FindAllChestsRPC()
    {
        if (GameManager.Instance && GameManager.Instance.player) {
            GameManager.Instance.player.playerCompass.FindAllChest();
            GameManager.Instance.player.playerCompass.FindMainGate();
            GameManager.Instance.player.playerCompass.FindEnemyBoss();
        }
    }

    private void LoadMainGateKeyAndQuestionDifficultyData(QuestionDifficulty questionDifficulty, MainGateKeyRaw mainGateRaw)
    {
        TeamType team = TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam());
        // GameManager.Instance.LoadFragmentsKey(MainGateKey.ConvertFromRawData(team, mainGateRaw));
        GameManager.Instance.LoadFragmentsKey(TeamType.Red, MainGateKey.ConvertFromRawData(TeamType.Red, mainGateRaw));
        GameManager.Instance.LoadFragmentsKey(TeamType.Blue, MainGateKey.ConvertFromRawData(TeamType.Blue, mainGateRaw));

        GameManager.Instance.LoadQuestionDifficulty(questionDifficulty);
    }

    private void PlayAsPlayer()
    {
        masterCamera.gameObject.SetActive(false);

        TeamType team = TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam());
        GameManager.Instance.BeginPlayer(team);
    }

    private void PlayAsSpectator()
    {
        GameManager.Instance.BeginSpectator();
    }

    public void EnableExitGameButton()
    {
        masterStopGameButton.SetActive(false);
        // masterExitGameButton.SetActive(true);
        masterExitGameButton.SetActive(false);
    }

    public void StopGame()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                pv.RPC("StopGameRPC", PhotonTargets.AllBuffered, ScoreManager.Instance.RedTeamScore, ScoreManager.Instance.BlueTeamScore);

                EnableExitGameButton();
            }
        } else
        {
            StopGameRPC(ScoreManager.Instance.RedTeamScore, ScoreManager.Instance.BlueTeamScore);

            EnableExitGameButton();
        }
    }

    [PunRPC]
    private void StopGameRPC(int redScore, int blueScore)
    {
        GameManager.Instance.AnnounceWinnerForced(redScore, blueScore);
    }

    public void DisconnectAllPlayer()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                pv.RPC("DisconnectAllPlayerRPC", PhotonTargets.AllBuffered);

                PhotonNetwork.LeaveRoom();

                Debug.Log("Loading Lobby");
                SceneManager.LoadScene("LobbyMenu");
            }
        } else
        {
            DisconnectAllPlayerRPC();

            Debug.Log("Loading Lobby");
            SceneManager.LoadScene("LobbyMenu");
        }
    }

    [PunRPC]
    private void DisconnectAllPlayerRPC()
    {
        GameManager.Instance.BackToLobby();

        WindowMaster.Instance.Show("Game was stopped by Master.");
    }

    [PunRPC]
    public void BackToLobbyMasterRPC()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                WindowMaster.Instance.Show("Leaving Game..");
            }
            else
            {
                WindowMaster.Instance.Show("Leaving Game..");
            }

            PhotonNetwork.LeaveRoom();
        }

        Debug.Log("Loading Lobby");
        SceneManager.LoadScene("LobbyMenu");
    }
}
