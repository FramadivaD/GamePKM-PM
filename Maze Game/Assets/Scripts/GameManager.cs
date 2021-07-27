using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Text pausePlayerAndTeamText;

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
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeam(player.teamType)) + "\">"
                + player.teamType
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

    private void SpawnPlayer(TeamType teamType)
    {
        GameObject p = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(), Quaternion.identity, 0);
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

    public void AnnounceWinner(TeamType winnerTeam)
    {
        WinnerWasAnnounced = true;
        WinnerTeam = winnerTeam;

        if (PhotonNetwork.player.IsMasterClient)
        {
            winnerTeamText.text = "TEAM "
                + "<color=\"#" + ColorUtility.ToHtmlStringRGB(TeamHelper.GetColorTeam(winnerTeam)) + "\">"
                + winnerTeam.ToString()
                + "</color>";

            winnerStatusText.text = "WINNER";
        }
        else
        {
            winnerTeamText.text = "TEAM " + winnerTeam.ToString();
            winnerStatusText.text = (winnerTeam == player.teamType) ? "WINNER" : "DEFEAT";

            gameplayUI.SetActive(false);
        }

        StartCoroutine(AnnounceWinnerUIAfter(winnerTeam, 3));
    }

    public IEnumerator AnnounceWinnerUIAfter(TeamType winnerTeam, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        winnerUI.SetActive(true);
    }

    public void BackToLobby()
    {
        Debug.Log("Loading Lobby");
        SceneManager.LoadScene("LobbyMenu");
    }

    public void BackToMainMenu()
    {
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
}
