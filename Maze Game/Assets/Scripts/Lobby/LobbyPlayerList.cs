using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LobbyPlayerList : Photon.PunBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private Transform playerSelection;

    [SerializeField] private PhotonPlayer[] players;

    [SerializeField] private LobbyPlayerPreview[] playersPreviewList;

    [SerializeField] private Button changeRedTeamButton;
    [SerializeField] private Button changeBlueTeamButton;

    private string playerMasterID;
    private string playerMasterName;

    private void Awake()
    {
        players = new PhotonPlayer[8];
    }

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
        if (PhotonNetwork.player.IsMasterClient)
        {
            LoadConnectedPlayers();
        }

        RefreshPreview();

        RefreshChangeTeamButton();
    }

    // Master only
    private void LoadConnectedPlayers()
    {
        if (PhotonNetwork.player.IsMasterClient)
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
                    players[redTeam] = photonPlayers[i];
                    redTeam++;
                }
                else if (photonPlayers[i].GetTeam() == PunTeams.Team.blue)
                {
                    players[4 + blueTeam] = photonPlayers[i];
                    blueTeam++;
                }
            }

            Debug.Log("End of Printing players..");

            Debug.Log("This Room Master Player ID : " + playerMasterID);
            Debug.Log("This Room Master Player Name : " + playerMasterName);
        }
    }

    // Called from master into all clients
    private void UpdateAllClientsPlayersData()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            pv.RPC("RetrievePlayersData", PhotonTargets.AllBuffered, players);

            RefreshPreview();
            RefreshChangeTeamButton();
        }
    }

    // Called into all clients from master
    [PunRPC]
    private void RetrievePlayersData(PhotonPlayer[] photonPlayers)
    {
        // if is client
        if (!PhotonNetwork.player.IsMasterClient)
        {
            players = photonPlayers;

            RefreshPreview();
            RefreshChangeTeamButton();
        }
    }

    // When a player connected to room
    private void RegisterPlayerTeam(PhotonPlayer player)
    {
        if (PhotonNetwork.player.IsMasterClient) {
            // if connected and not a master client and has no team then fill up
            if (!player.IsMasterClient && player.GetTeam() == PunTeams.Team.none)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (i % 2 == 0)
                    {
                        // red
                        if (players[i / 2] == null) {
                            players[i / 2] = player;
                            player.SetTeam(PunTeams.Team.red);
                            break;
                        }
                    } else
                    {
                        // blue
                        if (players[4 + i / 2] == null)
                        {
                            players[4 + i / 2] = player;
                            player.SetTeam(PunTeams.Team.blue);
                            break;
                        }
                    }
                }
            }
        }
    }

    // When a player disconnected from room
    private void UnregisterPlayerTeam(PhotonPlayer player)
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            // if connected and not a master client and has no team then fill up
            if (!player.IsMasterClient)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (players[i] == player)
                    {
                        players[i] = null;
                    }
                }
            }
        }
    }

    private void RefreshPreview()
    {
        Debug.Log("Refresh Preview");
        for (int i = 0; i < 8; i++)
        {
            if (players[i] != null)
            {
                Debug.Log("ID : " + players[i].ID + " : " + players[i].NickName);
                playersPreviewList[i].Initialize(players[i]);
            } else
            {
                playersPreviewList[i].Initialize(null);
            }
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
        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("A Player Connected");

            RegisterPlayerTeam(newPlayer);
            UpdateAllClientsPlayersData();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("A Player Disconnected");

            UnregisterPlayerTeam(otherPlayer);
            UpdateAllClientsPlayersData();
        }
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
        }
    }

    public void ChangeRedTeam()
    {
        ChangeMyTeam(TeamType.Red);
    }

    public void ChangeBlueTeam()
    {
        ChangeMyTeam(TeamType.Blue);
    }
}
