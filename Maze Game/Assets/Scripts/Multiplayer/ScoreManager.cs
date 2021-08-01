using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public PhotonView pv;

    public Text redTeamScoreText;
    public Text blueTeamScoreText;

    public int RedTeamScore { get; private set; }
    public int BlueTeamScore { get; private set; }

    private bool scoreLocked = false;

    private void Awake()
    {
        Instance = this;
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
        Color redColor = TeamHelper.GetColorTeamAlter((TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Red));
        Color blueColor = TeamHelper.GetColorTeamAlter((TeamType)TeamHelper.GetColorTeamAlterIndex(TeamType.Blue));

        redTeamScoreText.text = "<color=\"" + ColorUtility.ToHtmlStringRGB(redColor) + "\">" + RedTeamScore.ToString() + "</color>";
        blueTeamScoreText.text = "<color=\"" + ColorUtility.ToHtmlStringRGB(blueColor) + "\">" + BlueTeamScore.ToString() + "</color>";
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
