using UnityEngine;
using System.Collections.Generic;

public class TeamData
{
    public TeamType teamType { get; private set; }
    public MainGateKey FragmentsKey { get; }

    public TeamData(TeamType teamType, MainGateKey fragmentsKey)
    {
        this.FragmentsKey = fragmentsKey;

        this.teamType = teamType;
    }
}
