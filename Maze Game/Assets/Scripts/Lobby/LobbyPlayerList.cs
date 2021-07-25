using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerListContainer
{
    public List<PlayerListDetails> players;

    public PlayerListContainer()
    {
        players = new List<PlayerListDetails>();
    }
}

[System.Serializable]
public class PlayerListDetails
{
    public bool playerExist = false;
    public string playerID;
    public string playerName;

    public PlayerListDetails(bool playerExist, string playerID = "", string playerName = "")
    {
        this.playerExist = playerExist;
        this.playerID = playerID;
        this.playerName = playerName;
    }
}

public class LobbyPlayerList : Photon.PunBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private Transform playerSelection;

    [SerializeField] private PlayerListContainer players;

    [SerializeField] private LobbyPlayerPreview[] playersPreviewList;

    [SerializeField] private Button changeRedTeamButton;
    [SerializeField] private Button changeBlueTeamButton;

    private string playerMasterID;
    private string playerMasterName;

    private void Awake()
    {
        for (int i = 0;i < 8; i++)
        {
            playersPreviewList[i].Initialize(null);
        }

        players = CreatePlayers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Initialize();
        } else if (Input.GetKeyDown(KeyCode.G))
        {
            RefreshPreview();
        } else if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateAllClientsPlayersData();
        }
    }

    public void Initialize()
    {
        // Kalau client maka, beritahu master kalo si client ini berhasil login
        if (!PhotonNetwork.player.IsMasterClient)
        {
            OnPlayerSendingUsernameAsClient();
        } else
        {
            RefreshPreview();
            RefreshChangeTeamButton();
        }
    }

    // Called from master into all clients
    private void UpdateAllClientsPlayersData()
    {
        Debug.Log("Try to send player list from master to all clients");
        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("Because we are master so proceed.");

            if (players == null) players = CreatePlayers();

            string content = JsonUtility.ToJson(players);

            Debug.Log("Player JSON : ");
            Debug.Log(content);

            Debug.Log("Start sending data.");
            pv.RPC("RetrievePlayersData", PhotonTargets.AllBuffered, content);

            RefreshPreview();
            RefreshChangeTeamButton();
        }
        else
        {
            Debug.Log("Because we are not master (client) so don't proceed.");
        }
    }

    // Called into all clients from master
    [PunRPC]
    private void RetrievePlayersData(string playerListDetailsListJson)
    {
        // if is client
        Debug.Log("Try to receive master rpc request.");
        if (!PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("Because we are client so proceed.");

            if (players == null) players = CreatePlayers();
            players = JsonUtility.FromJson<PlayerListContainer>(playerListDetailsListJson);

            Debug.Log("Player data received.");

            RefreshPreview();
            RefreshChangeTeamButton();
        } else
        {
            Debug.Log("Because we are not client (master) so don't proceed.");
        }
    }

    // When a player connected to room
    private void RegisterPlayerTeam(PhotonPlayer player)
    {
        if (players == null) players = CreatePlayers();
        if (PhotonNetwork.player.IsMasterClient) {
            // if connected and not a master client and has no team then fill up
            if (!player.IsMasterClient) {
                if (player.GetTeam() == PunTeams.Team.none)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // red
                            if (players.players[i / 2] == null || players.players[i / 2].playerExist == false) {
                                players.players[i / 2] = new PlayerListDetails(true, player.UserId, player.NickName);
                                player.SetTeam(PunTeams.Team.red);
                                break;
                            }
                        } else
                        {
                            // blue
                            if (players.players[4 + i / 2] == null || players.players[4 + i / 2].playerExist == false)
                            {
                                players.players[4 + i / 2] = new PlayerListDetails(true, player.UserId, player.NickName);
                                player.SetTeam(PunTeams.Team.blue);
                                break;
                            }
                        }
                    }
                }
                else if (player.GetTeam() == PunTeams.Team.red)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (players.players[i] == null || players.players[i].playerExist == false)
                        {
                            players.players[i] = new PlayerListDetails(true, player.UserId, player.NickName);
                            player.SetTeam(PunTeams.Team.red);
                            break;
                        }
                    }
                }
                else if (player.GetTeam() == PunTeams.Team.blue)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (players.players[4 + i] == null || players.players[4 + i].playerExist == false)
                        {
                            players.players[4 + i] = new PlayerListDetails(true, player.UserId, player.NickName);
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
        if (players == null) players = CreatePlayers();
        if (PhotonNetwork.player.IsMasterClient)
        {
            // if connected and not a master client and has no team then fill up
            if (!player.IsMasterClient)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (players.players[i].playerID == player.UserId)
                    {
                        players.players[i] = null;
                    }
                }
            }
        }
    }

    private void RefreshPreview()
    {
        Debug.Log("Refresh Preview");
        if (players == null) players = CreatePlayers();
        for (int i = 0; i < 8; i++)
        {
            if (players.players[i] != null && players.players[i].playerExist)
            {
                Debug.Log("ID : " + players.players[i].playerID + " : " + players.players[i].playerName);
                playersPreviewList[i].Initialize(players.players[i]);
            } else
            {
                players.players[i] = new PlayerListDetails(false);
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

    public void OnPlayerSendingUsernameAsClient()
    {
        // jika client maka
        if (!PhotonNetwork.player.IsMasterClient)
        {
            // lakukan register player ini ke server
            pv.RPC("SendPlayerFromClientToMaster", PhotonTargets.AllBuffered, PhotonNetwork.player);
        }
    }

    [PunRPC]
    private void SendPlayerFromClientToMaster(PhotonPlayer player)
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            RegisterPlayerTeam(player);
            UpdateAllClientsPlayersData();
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("A Player Connected");
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

    private PlayerListContainer CreatePlayers()
    {
        PlayerListContainer c = new PlayerListContainer();        

        for (int i = 0;i < 8; i++)
        {
            c.players.Add(new PlayerListDetails(false));
        }

        return c;
    }
}
