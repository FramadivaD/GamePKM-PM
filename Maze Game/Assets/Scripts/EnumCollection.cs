using UnityEngine;
using System.Collections;

public enum PlayerInteractionType
{
    None,
    OpenChest,
    OpenGate
}

[System.Serializable]
public enum TeamType
{
    Red, Blue
}

public enum BossAttackType
{
    None, Punch, ProjectileLaunch, ProjectileBulletHell
}

public static class TeamHelper
{
    public static readonly Color[] TeamColor = {
        new Color(255, 0, 0), // Red
        new Color(0, 0, 255) // Blue
    };

    public static readonly TeamType[] TeamTypes =
    {
        TeamType.Red,
        TeamType.Blue
    };

    public static int GetTeamCount()
    {
        return TeamTypes.Length;
    }

    public static Color GetColorTeam(TeamType teamType)
    {
        return TeamColor[(int) teamType];
    }
}