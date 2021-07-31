using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomTeamChatText : MonoBehaviour
{
    Text messageText;

    string username;
    string message;
    string teamType;

    public void Initialize(string username, string message, int teamTypeInt)
    {
        string content = BuildMessageContent(username, message, teamTypeInt);

        messageText.text = content;
    }

    private string BuildMessageContent(string username, string message, int teamTypeInt)
    {
        string content = "";

        content += "<color=\"#" + TeamHelper.TeamColorAlter[teamTypeInt] + "\"";
        content += username;
        content += "</color>";
        content += " : ";
        content += message;

        return content;
    }
}
