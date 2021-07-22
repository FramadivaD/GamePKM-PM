using UnityEngine;
using System.Collections.Generic;

public class TeamData
{
    public TeamType teamType { get; private set; }
    private readonly List<Player> players;
    public MainGateKey FragmentsKey { get; }

    public int PlayersCount
    {
        get
        {
            return players.Count;
        }
    }

    public TeamData(TeamType teamType, MainGateKey fragmentsKey)
    {
        players = new List<Player>();
        this.FragmentsKey = fragmentsKey;

        this.teamType = teamType;
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}
