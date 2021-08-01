using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public PhotonView pv;

    public Text scoreText;    

    public int RedTeamScore { get; private set; }
    public int BlueTeamScore { get; private set; }

    private bool scoreLocked = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        RefreshScoreUI();
    }

    public void AddScore(TeamType teamType, int value)
    {
        if (PhotonNetwork.connected) {
            pv.RPC("AddScoreRPC", PhotonTargets.AllBuffered, (int)teamType, value);
        } else
        {
            AddScoreRPC((int) teamType, value);
        }
    }

    [PunRPC]
    private void AddScoreRPC(int teamTypeInt, int value)
    {
        if (!scoreLocked)
        {
            TeamType teamType = (TeamType)teamTypeInt;

            if (teamType == TeamType.Red)
            {
                RedTeamScore += value;
            }
            else if (teamType == TeamType.Blue)
            {
                BlueTeamScore += value;
            }

            RefreshScoreUI();
        }
    }

    private void RefreshScoreUI()
    {
        TeamType redTeam = (TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Red);
        TeamType blueTeam = (TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Blue);

        Color redColor = TeamHelper.GetColorTeamAlter(TeamType.Red);
        Color blueColor = TeamHelper.GetColorTeamAlter(TeamType.Blue);

        string content = "<color=\"#" + ColorUtility.ToHtmlStringRGB(redColor) + "\">" + redTeam.ToString() + "</color>";
        content += "\n";
        content += RedTeamScore.ToString() + " PTS";
        content += "\n";

        content += "<color=\"#" + ColorUtility.ToHtmlStringRGB(blueColor) + "\">" + blueTeam.ToString() + "</color>";
        content += "\n";
        content += BlueTeamScore.ToString() + " PTS";
        content += "\n";

        scoreText.text = content;
    }

    public void LockScore()
    {
        scoreLocked = true;
    }

    // kalo -1 draw, 0 red, 1 blue
    public int CheckWinner()
    {
        if (RedTeamScore > BlueTeamScore)
        {
            return (int)TeamType.Red;
        } else if (RedTeamScore < BlueTeamScore)
        {
            return (int)TeamType.Blue;
        }
        return -1;
    }
}
