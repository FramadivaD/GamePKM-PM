using UnityEngine;
using System.Collections.Generic;

public class TeamData
{
    public TeamType teamType { get; private set; }
    private readonly List<Player> players;

    public int PlayersCount
    {
        get
        {
            return players.Count;
        }
    }

    public TeamData(TeamType teamType)
    {
        this.teamType = teamType;
        players = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}
