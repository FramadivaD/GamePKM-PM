using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomTeamChatText : MonoBehaviour
{
    [SerializeField] private Text messageText;

    bool isPrivateTeam = false;
    string username;
    string message;
    int teamTypeInt;

    public void Initialize(bool isPrivateTeam, string username, string message, int teamTypeInt)
    {
        this.isPrivateTeam = isPrivateTeam;
        this.username = username;
        this.message = message;
        this.teamTypeInt = teamTypeInt;

        messageText.text = BuildMessageContent(isPrivateTeam, username, message, teamTypeInt);
    }

    private string BuildMessageContent(bool isPrivateTeam, string username, string message, int teamTypeInt)
    {
        string content = "";

        string teamColor = (teamTypeInt < 0 ? "FFFFFF" : ColorUtility.ToHtmlStringRGB(TeamHelper.TeamColorAlter[teamTypeInt]));

        if (teamTypeInt < 0)
        {
            username = "MASTER";
        }

        content += "<color=\"#" + teamColor + "\">";
        content += username;
        content += "</color>";

        content += " to ";

        if (isPrivateTeam)
        {
            content += "<color=\"#" + teamColor + "\">";
            content += "Teammates";
            content += "</color>";
            content += " : ";
        } else
        {
            content += "Everyone : ";
        }

        content += "\n";

        content += message;

        return content;
    }
}
