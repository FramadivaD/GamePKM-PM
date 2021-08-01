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

        content += "<color=\"#" + TeamHelper.TeamColorAlter[teamTypeInt] + "\"";
        content += username;
        content += "</color>";

        content += " to ";

        if (isPrivateTeam)
        {
            content += "<color=\"#" + TeamHelper.TeamColorAlter[teamTypeInt] + "\"";
            content += "Teammates";
            content += "</color>";
        } else
        {
            content += "Everyone :";
        }

        content += "\n";

        content += message;

        return content;
    }
}
