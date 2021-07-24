using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LobbyPlayerList : Photon.PunBehaviour
{
    [SerializeField] private Transform playerSelection;

    [SerializeField] private string[] playersJoinedID;
    [SerializeField] private string[] playersJoinedName;

    [SerializeField] private LobbyPlayerPreview[] playersPreviewList;

    [SerializeField] private Button changeRedTeamButton;
    [SerializeField] private Button changeBlueTeamButton;

    private string playerMasterID;
    private string playerMasterName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Initialize();
        } else if (Input.GetKeyDown(KeyCode.G))
        {
            RefreshPreview();
        }
    }

    public void Initialize()
    {
        LoadConnectedPlayers();

        RefreshPreview();

        RefreshChangeTeamButton();
    }

    private void LoadConnectedPlayers()
    {
        // retrieve network connected players
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;

        int redTeam = 0;
        int blueTeam = 0;

        Debug.Log("Printing players..");

        for (int i = 0; i < photonPlayers.Length; i++)
        {
            Debug.Log("Player : " + photonPlayers[i].NickName);

            if (photonPlayers[i].IsMasterClient)
            {
                playerMasterID = photonPlayers[i].UserId;
                playerMasterName = photonPlayers[i].NickName;
            }
            else if (photonPlayers[i].GetTeam() == PunTeams.Team.red)
            {
                playersJoinedID[redTeam] = photonPlayers[i].UserId;
                playersJoinedName[redTeam] = photonPlayers[i].NickName;
                redTeam++;
            }
            else if (photonPlayers[i].GetTeam() == PunTeams.Team.blue)
            {
                playersJoinedID[4 + blueTeam] = photonPlayers[i].UserId;
                playersJoinedName[4 + blueTeam] = photonPlayers[i].NickName;
                blueTeam++;
            }
        }

        Debug.Log("End of Printing players..");

        // if connected and not a master client and has no team then fill up
        if (!PhotonNetwork.player.IsMasterClient && PhotonNetwork.player.GetTeam() == PunTeams.Team.none)
        {
            if (redTeam < 4 && redTeam <= blueTeam) {
                PhotonNetwork.player.SetTeam(PunTeams.Team.red);
                playersJoinedID[redTeam] = PhotonNetwork.player.UserId;
                playersJoinedName[redTeam] = PhotonNetwork.player.NickName;
                redTeam++;
            } else if (blueTeam < 4)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
                playersJoinedID[4 + blueTeam] = PhotonNetwork.player.UserId;
                playersJoinedName[4 + blueTeam] = PhotonNetwork.player.NickName;
                blueTeam++;
            }
        }

        // Fill up the rest with null player
        for (; redTeam < 4; redTeam++)
        {
            playersJoinedID[redTeam] = "";
            playersJoinedName[redTeam] = "";
        }

        for (; blueTeam < 4; blueTeam++)
        {
            playersJoinedID[4 + blueTeam] = "";
            playersJoinedName[4 + blueTeam] = "";
        }

        Debug.Log("This Room Master Player ID : " + playerMasterID);
        Debug.Log("This Room Master Player Name : " + playerMasterName);
    }

    private void RefreshPreview()
    {
        Debug.Log("Refresh Preview");
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("ID : " + playersJoinedName[i] + " : " + playersJoinedName[i]);
            playersPreviewList[i].Initialize(playersJoinedID[i], playersJoinedName[i]);
        }
        Debug.Log("End of Refresh Preview");
    }

    private void RefreshChangeTeamButton()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            changeRedTeamButton.gameObject.SetActive(false);
            changeBlueTeamButton.gameObject.SetActive(false);
        }
        else if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
        {
            changeRedTeamButton.gameObject.SetActive(false);
            changeBlueTeamButton.gameObject.SetActive(true);
        }
        else if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
        {
            changeRedTeamButton.gameObject.SetActive(true);
            changeBlueTeamButton.gameObject.SetActive(false);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("A Player Connected");

        Initialize();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("A Player Disconnected");

        Initialize();
    }

    private void ChangeMyTeam(TeamType teamType)
    {
        if (!PhotonNetwork.player.IsMasterClient) {
            if (teamType == TeamType.Red)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            } else if (teamType == TeamType.Blue)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            }

            // photonView.RPC("BroadcastChangeTeam", PhotonTargets.AllBufferedViaServer);
        }
    }

    /*
    [PunRPC]
    private void BroadcastChangeTeam()
    {
        Initialize();
    }
    */

    public void ChangeRedTeam()
    {
        ChangeMyTeam(TeamType.Red);
    }

    public void ChangeBlueTeam()
    {
        ChangeMyTeam(TeamType.Blue);
    }
}
