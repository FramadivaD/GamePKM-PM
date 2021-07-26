using UnityEngine;
using System.Collections;

public class MultiplayerNetworkMaster : Photon.PunBehaviour
{
    public static MultiplayerNetworkMaster Instance { get; private set; }

    public Camera masterCamera;

    public PhotonView pv;

    public QuestionDifficulty questionDifficulty;
    public MainGateKeyRaw mainGateRaw;

    [SerializeField] private FirebaseManager firebaseManager;

    private int playerSceneCount = 0;
    private bool masterClientInitialized = false;

    public bool testClientSingle;

    private void Awake()
    {
        Instance = this;
    }

    private void TestClientSingle()
    {
        if (testClientSingle)
        {
            MainGateKey testKey = new MainGateKey(TeamType.Red);

            testKey.AddFragment(new MainGateFragment(testKey, "Kucing", "Data1"));
            testKey.AddFragment(new MainGateFragment(testKey, "Sapi", "Data2"));
            testKey.AddFragment(new MainGateFragment(testKey, "Kadal", "Data3"));

            GameManager.Instance.LoadFragmentsKey(testKey);
            GameManager.Instance.LoadQuestionDifficulty(new QuestionDifficulty() { bangunDatar = true, pecahan = true, penjumlahan = true, perkalian = true, persamaan = true });

            PhotonNetwork.ConnectUsingSettings(Application.version);
            GameManager.Instance.GenerateLevel();
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
            GameManager.Instance.GenerateLevel();

            PlayAsPlayer();
        }
    }

    private void Start()
    {
        if (!testClientSingle)
        {
            if (!PhotonNetwork.player.IsMasterClient)
            {
                pv.RPC("AddClientPlayer", PhotonTargets.MasterClient);
            }
            else
            {
                Debug.Log("Waiting Player Count : " + playerSceneCount + "/" + PhotonNetwork.playerList.Length);
                AddClientPlayer();
            }
        } else
        {
            Invoke("TestClientSingle", 1);
        }
    }

    [PunRPC]
    private void AddClientPlayer()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            playerSceneCount++;

            Debug.Log("A Player Joined Scene.");
            Debug.Log("Waiting Player Count : " + playerSceneCount + "/" + PhotonNetwork.playerList.Length);

            if (playerSceneCount >= PhotonNetwork.playerList.Length)
            {
                Debug.Log("Player Count Satisfied, Initialize Master Client.");

                InitializeMasterClient();
            }
        }
    }

    private void InitializeMasterClient()
    {
        string diffJson = LobbyTeacherRoomQuestionDifficulty.SelectedDifficultyJson;
        string keyJsonDownloadURL = LobbyTeacherRoom.MainGateDownloadURL;

        Debug.Log("Karena master jadi akan ngirim data");

        pv.RPC("ReceiveMainGateKeyDownloadURLAndQuestionDifficultyData", PhotonTargets.AllBuffered, diffJson, keyJsonDownloadURL);
    }

    [PunRPC]
    private void ReceiveMainGateKeyDownloadURLAndQuestionDifficultyData(string diffJson, string keyJsonDownloadURL)
    {
        firebaseManager.DownloadData(keyJsonDownloadURL, 
            (byte[] data) => {
                string keyJson = System.Text.Encoding.ASCII.GetString(data);

                Debug.Log("Download MainGateKey Data Success!");

                Debug.Log("Download URL : " + keyJsonDownloadURL);

                ReceiveMainGateKeyAndQuestionDifficultyData(diffJson, keyJson);
            }, 
            () => {
                Debug.Log("Download MainGateKey Data Failed.. Sadge.");

                Debug.Log("Download URL : " + keyJsonDownloadURL);
            }
        );
    }

    private void ReceiveMainGateKeyAndQuestionDifficultyData(string diffJson, string keyJson)
    {
        Debug.Log("Meskipun master / client akan menerima data.");

        questionDifficulty = JsonUtility.FromJson<QuestionDifficulty>(diffJson);
        mainGateRaw = JsonUtility.FromJson<MainGateKeyRaw>(keyJson);

        LoadMainGateKeyAndQuestionDifficultyData(questionDifficulty, mainGateRaw);

        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("Karena master maka dia yang generate level, biar client bisa menyesuaikan.");
            GameManager.Instance.GenerateLevel();

            Debug.Log("Karena master maka akan main sebagai spectator.");
            PlayAsSpectator();
        }
        else
        {
            Debug.Log("Karena client maka akan main sebagai player.");

            PlayAsPlayer();
        }
    }

    private void LoadMainGateKeyAndQuestionDifficultyData(QuestionDifficulty questionDifficulty, MainGateKeyRaw mainGateRaw)
    {
        TeamType team = TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam());
        GameManager.Instance.LoadFragmentsKey(MainGateKey.ConvertFromRawData(team, mainGateRaw));

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
}
