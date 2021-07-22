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

    private Dictionary<TeamType, TeamData> playerTeam;
    private List<Player> players;

    private void Awake()
    {
        roomGenerator.RandomizeMap();
    }

    private void Start()
    {
        Instance = this;

        AllowEntityMove = false;
        player.login.OnSubmitNameSuccess += EnableAllEntitiesMovement;

        RegisterPlayer();
    }

    private void OnDestroy()
    {
        player.login.OnSubmitNameSuccess -= EnableAllEntitiesMovement;
    }

    public void EnableAllEntitiesMovement(string str)
    {
        AllowEntityMove = true;
    }

    public void RegisterPlayer()
    {
        // Make playerTeam
        if (playerTeam == null) playerTeam = new Dictionary<TeamType, TeamData>();

        if (playerTeam.TryGetValue(player.teamType, out TeamData teamData))
        {
            teamData.AddPlayer(player);
        } else
        {
            TeamData newTeamData = new TeamData(player.teamType);
            newTeamData.AddPlayer(player);
            playerTeam[player.teamType] = newTeamData;
        }

        // make players
        if (players == null) players = new List<Player>();
        players.Add(player);
    }
}
