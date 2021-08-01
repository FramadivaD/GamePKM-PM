using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Extensione.Audio;
using Extensione.Window;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player player;
    public Player playerPrefab;

    public bool AllowEntityMove { get; private set; } = true;
    public bool AllowEnemyMove { get; private set; } = true;
    public bool AllowPlayerMove { get; private set; } = true;

    public RoomGenerator roomGenerator;

    public static Dictionary<TeamType, TeamData> PlayersTeam { get; private set; }
    private List<Player> players;

    public bool WinnerWasAnnounced { get; private set; }
    public TeamType WinnerTeam { get; private set; }

    [Header("All about UI")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject winnerUI;
    [SerializeField] private Text winnerStatusText;
    [SerializeField] private Text winnerTeamText;
    [SerializeField] private GameObject scoreManagerUI;
    [SerializeField] private GameObject chatRoomUI;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Text pausePlayerAndTeamText;

    [Header("Audio")]
    [SerializeField] private AudioClip epicMusic;
    [SerializeField] private AudioClip winSFX;

    private bool isPaused = false;
    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
        private set
        {
            isPaused = value;

            pauseUI.SetActive(isPaused);
            pausePlayerAndTeamText.text =
                player.PlayerName
                + " : "
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(player.teamType)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(player.teamType)).ToString()
                + "</color>"
                + " Team";
        }
    }

    private void Awake()
    {
        Instance = this;

        PlayersTeam = new Dictionary<TeamType, TeamData>();

        winnerUI.SetActive(false);

        isPaused = false;

        if (MultiplayerNetworkMaster.Instance) {
            EnableAllEntitiesMovement(null);
        } else
        {
            roomGenerator.RandomizeMap();
        }
    }

    public void EnableAllEntitiesMovement(string str)
    {
        AllowEntityMove = true;
    }

    public void DisableAllEntitiesMovement(string str)
    {
        AllowEntityMove = false;
    }

    private void SpawnPlayer(TeamType teamType)
    {
        Vector3 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-3.6f, 3.6f));

        GameObject p = PhotonNetwork.Instantiate(playerPrefab.name, randPos, Quaternion.identity, 0);
        player = p.GetComponent<Player>();

        pauseUI = player.PauseUI;
        pausePlayerAndTeamText = player.PauseTeamText;
        gameplayUI = player.GameplayUI;

        player.ChangeDisplayName(PhotonNetwork.player.NickName);

        pauseUI.SetActive(false);

        player.Initialize(teamType);
    }

    private void RegisterTeam(TeamType teamType, MainGateKey key)
    {
        if (PlayersTeam == null) PlayersTeam = new Dictionary<TeamType, TeamData>();

        if (!PlayersTeam.ContainsKey(teamType)) {
            PlayersTeam[teamType] = new TeamData(teamType, key);
        }
    }

    public void LoadFragmentsKey(TeamType teamType, MainGateKey mainGateKey)
    {
        RegisterTeam(teamType, mainGateKey);
    }

    public void LoadQuestionDifficulty(QuestionDifficulty difficulty)
    {
        // TODO : Tambahi untuk check difficulty question
    }

    public void GenerateLevel()
    {
        roomGenerator.RandomizeMap();
    }

    public void BeginPlayer(TeamType teamType)
    {
        // Spawn player dulu, karena pas baru mulai gaada player

        SpawnPlayer(teamType);
    }

    public void BeginSpectator()
    {
        // Untuk si Guru nanti jadi spectator
    }

    public void AnnounceWinnerForced(int redTeamScore, int blueTeamScore)
    {
        WinnerWasAnnounced = true;

        string content = "TEAM "
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(TeamType.Red)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Red)).ToString()
                + "</color>";

        content += " and TEAM "
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(TeamType.Blue)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Blue)).ToString()
                + "</color>";

        content += "\n";

        // content += ScoreManager.Instance.RedTeamScore + " vs " + ScoreManager.Instance.BlueTeamScore;
        content += redTeamScore + " vs " + blueTeamScore;

        winnerTeamText.text = content;


        if (!PhotonNetwork.player.IsMasterClient) {
            if (redTeamScore == blueTeamScore)
            {
                winnerStatusText.text = "DRAW";
            }
            else if (redTeamScore > blueTeamScore)
            {
                if (player.teamType == TeamType.Red)
                {
                    winnerStatusText.text = "WINNER";
                } else
                {
                    winnerStatusText.text = "DEFEAT";
                }
            }
            else if (redTeamScore < blueTeamScore)
            {
                if (player.teamType == TeamType.Blue)
                {
                    winnerStatusText.text = "WINNER";
                }
                else
                {
                    winnerStatusText.text = "DEFEAT";
                }
            }
        } else
        {
            if (redTeamScore == blueTeamScore)
            {
                winnerStatusText.text = "DRAW";
            }
            else if (redTeamScore > blueTeamScore)
            {
                winnerStatusText.text =
                "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(TeamType.Red)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Red)).ToString()
                + "</color> WIN";
            }
            else if (redTeamScore < blueTeamScore)
            {
                winnerStatusText.text =
                "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(TeamType.Blue)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Blue)).ToString()
                + "</color> WIN";
            }
        }

        if (!PhotonNetwork.player.IsMasterClient)
        {
            gameplayUI.SetActive(false);
            scoreManagerUI.SetActive(false);
            chatRoomUI.SetActive(false);
        }

        StartCoroutine(AnnounceWinnerUIForcedAfter(3));
    }

    public void AnnounceWinner(TeamType winnerTeam)
    {
        WinnerWasAnnounced = true;
        WinnerTeam = winnerTeam;

        MultiplayerNetworkMaster.Instance.EnableExitGameButton();

        if (PhotonNetwork.player.IsMasterClient)
        {
            winnerTeamText.text = "TEAM "
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(winnerTeam)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(winnerTeam)).ToString()
                + "</color>";

            winnerStatusText.text = "WINNER";
        }
        else
        {
            winnerTeamText.text = "TEAM "
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeamAlter(player.teamType)) + "\">"
                + ((TeamType)TeamHelper.GetColorTeamAlterIndex(player.teamType)).ToString()
                + "</color>";

            winnerStatusText.text = (winnerTeam == player.teamType) ? "WINNER" : "DEFEAT";

            gameplayUI.SetActive(false);
            scoreManagerUI.SetActive(false);
            chatRoomUI.SetActive(false);
        }

        StartCoroutine(AnnounceWinnerUIAfter(winnerTeam, 3));
    }

    public IEnumerator AnnounceWinnerUIAfter(TeamType winnerTeam, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        winnerUI.SetActive(true);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFXOnce(winSFX);
    }

    public IEnumerator AnnounceWinnerUIForcedAfter(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        winnerUI.SetActive(true);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFXOnce(winSFX);
    }

    public void BackToLobby()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                MultiplayerNetworkMaster.Instance.pv.RPC("BackToLobbyMasterRPC", PhotonTargets.AllBuffered);
            } else
            {
                // MultiplayerNetworkMaster.Instance.pv.RPC("BackToLobbyClientRPC", PhotonTargets.MasterClient, PhotonNetwork.player.NickName);

                MultiplayerNetworkMaster.Instance.BackToLobbyMasterRPC();
            }
        } else
        {
            MultiplayerNetworkMaster.Instance.BackToLobbyMasterRPC();
        }
    }

    public void BackToMainMenu()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.LeaveRoom();
        }
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPauseMenu()
    {
        IsPaused = true;
    }

    public void HidePauseMenu()
    {
        IsPaused = false;
    }

    public void PlayEpicMusic()
    {
        AudioManager.Instance.ChangeMusicSilent(epicMusic);
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                WindowMaster.Instance.Show(otherPlayer.NickName + " Leave the Game");
            }
        }
    }
}
