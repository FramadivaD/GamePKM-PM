using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player player;

    public bool AllowEntityMove { get; private set; } = true;
    public bool AllowEnemyMove { get; private set; } = true;
    public bool AllowPlayerMove { get; private set; } = true;

    public RoomGenerator roomGenerator;

    public static Dictionary<TeamType, TeamData> PlayersTeam { get; private set; }
    private List<Player> players;
    [SerializeField] private List<MainGateKey> fragmentsKey; //ada 2, sesuai dengan jumlah tim

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
        PlayersTeam = new Dictionary<TeamType, TeamData>();

        winnerUI.SetActive(false);
        pauseUI.SetActive(false);

        isPaused = false;
    }

    private void Start()
    {
        Instance = this;
    }

    public void EnableAllEntitiesMovement(string str)
    {
        AllowEntityMove = true;
    }

    private void RegisterTeam()
    {
        if (PlayersTeam == null) PlayersTeam = new Dictionary<TeamType, TeamData>();

        if (!PlayersTeam.ContainsKey(player.teamType)) {
            MainGateKey key = fragmentsKey[(int)player.teamType];

            TeamData newTeamData = new TeamData(player.teamType, key);
            PlayersTeam[player.teamType] = newTeamData;
        }
    }

    private void RegisterPlayer()
    {
        if (PlayersTeam.TryGetValue(player.teamType, out TeamData teamData))
        {
            teamData.AddPlayer(player);
        }

        // make players
        if (players == null) players = new List<Player>();
        players.Add(player);
    }

    public void LoadFragmentsKey(MainGateKey mainGateKey)
    {
        fragmentsKey = new List<MainGateKey>();
        fragmentsKey.Add(mainGateKey);
        fragmentsKey.Add(mainGateKey);
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

        RegisterTeam();
        RegisterPlayer();
    }

    public void BeginSpectator()
    {
        // Untuk si Guru nanti jadi spectator
    }

    public void AnnounceWinner(TeamType winnerTeam)
    {
        WinnerWasAnnounced = true;
        WinnerTeam = winnerTeam;

        winnerTeamText.text = "TEAM " + winnerTeam.ToString();
        winnerStatusText.text = (winnerTeam == player.teamType) ? "WINNER" : "DEFEAT";

        gameplayUI.SetActive(false);

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
