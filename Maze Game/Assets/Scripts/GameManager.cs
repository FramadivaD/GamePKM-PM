using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Awake()
    {
        PlayersTeam = new Dictionary<TeamType, TeamData>();

        InitializeFragmentsKey();

        RegisterTeam();
        RegisterPlayer();
    }

    private void Start()
    {
        Instance = this;

        AllowEntityMove = false;
        player.login.OnSubmitNameSuccess += EnableAllEntitiesMovement;

        roomGenerator.RandomizeMap();
    }

    private void OnDestroy()
    {
        player.login.OnSubmitNameSuccess -= EnableAllEntitiesMovement;
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

    private void InitializeFragmentsKey()
    {
        fragmentsKey = new List<MainGateKey>();
        MainGateKey redTeamKey = new MainGateKey(player.teamType);
        redTeamKey.AddFragment(new MainGateFragment(redTeamKey, "red_01", "Kucing"));
        redTeamKey.AddFragment(new MainGateFragment(redTeamKey, "red_02", "Sapi"));
        redTeamKey.AddFragment(new MainGateFragment(redTeamKey, "red_03", "Kuda"));
        fragmentsKey.Add(redTeamKey);
    }
}
