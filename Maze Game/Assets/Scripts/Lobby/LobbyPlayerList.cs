using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyPlayerList : Photon.PunBehaviour
{
    [SerializeField] private Transform playerSelection;

    [SerializeField] private string[] playersJoinedID;
    [SerializeField] private string[] playersJoinedName;

    [SerializeField] private LobbyPlayerPreview[] playersPreviewList;

    private string playerMasterID;
    private string playerMasterName;

    public void Initialize()
    {
        // retrieve network connected players
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;

        int redTeam = 0;
        int blueTeam = 0;

        for (int i = 0; i < photonPlayers.Length; i++)
        {
            if (photonPlayers[i].GetTeam() == PunTeams.Team.red)
            {
                playersJoinedID[redTeam] = photonPlayers[i].UserId;
                playersJoinedName[redTeam] = photonPlayers[i].NickName;
                redTeam++;
            } else if (photonPlayers[i].GetTeam() == PunTeams.Team.blue)
            {
                playersJoinedID[4 + blueTeam] = photonPlayers[i].UserId;
                playersJoinedName[4 + blueTeam] = photonPlayers[i].NickName;
                blueTeam++;
            } else if (photonPlayers[i].IsMasterClient)
            {
                playerMasterID = photonPlayers[i].UserId;
                playerMasterName = photonPlayers[i].NickName;
            }
        }

        for (; redTeam < 4; redTeam++)
        {
            playersJoinedID[redTeam] = null;
            playersJoinedName[redTeam] = null;
        }

        for (; blueTeam < 4; blueTeam++)
        {
            playersJoinedID[4 + blueTeam] = null;
            playersJoinedName[4 + blueTeam] = null;
        }

        Debug.Log("This Room Master ID : " + playerMasterID);
        Debug.Log("This Room Master Name : " + playerMasterName);

        RefreshPreview();
    }

    public void RefreshPreview()
    {
        for (int i = 0; i < 8;i++)
        {
            playersPreviewList[i].Initialize(playersJoinedID[i], playersJoinedName[i]);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Initialize();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Initialize();
    }
}
