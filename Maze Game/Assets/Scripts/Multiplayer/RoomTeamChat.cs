using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomTeamChat
{
    public PhotonView pv;

    [SerializeField] private RoomTeamChatText roomChatTextPrefab;

    [SerializeField] private Transform everyoneChatContainer;
    [SerializeField] private Transform privateTeamChatContainer;

    private bool receivePrivateTeamChat = false;

    private void Start()
    {
        
    }

    public void Initialize()
    {
        receivePrivateTeamChat = false;

        everyoneChatContainer.gameObject.SetActive(false);
        privateTeamChatContainer.gameObject.SetActive(false);

        if (PhotonNetwork.connected)
        {
            InitializeEveryoneChat();
            if (!PhotonNetwork.player.IsMasterClient)
            {
                InitializePrivateTeamChat();
            }
        }
    }

    private void InitializeEveryoneChat()
    {
        everyoneChatContainer.gameObject.SetActive(true);
    }

    private void InitializePrivateTeamChat()
    {
        privateTeamChatContainer.gameObject.SetActive(true);

        receivePrivateTeamChat = true;
    }

    public void SendPrivateTeamChat(string message)
    {
        if (receivePrivateTeamChat)
        {
            if (PhotonNetwork.connected)
            {
                string username = PhotonNetwork.player.NickName;

                if (!PhotonNetwork.player.IsMasterClient)
                {
                    int teamTypeInt =
                    TeamHelper.GetColorTeamAlterIndex(
                        TeamHelper.FromPhotonTeam(
                            PhotonNetwork.player.GetTeam()
                        )
                    );
                    pv.RPC("ReceivePrivateTeamChat", PhotonTargets.AllBuffered, username, message, teamTypeInt);
                }
            }
        }
    }

    public void SendEveryoneChat(string message)
    {
        if (PhotonNetwork.connected)
        {
            string username = PhotonNetwork.player.NickName;

            if (PhotonNetwork.player.IsMasterClient)
            {
                int teamTypeInt = -1;
                pv.RPC("ReceiveEveryoneChat", PhotonTargets.AllBuffered, username, message, teamTypeInt);
            }
            else
            {
                int teamTypeInt = 
                    TeamHelper.GetColorTeamAlterIndex(
                        TeamHelper.FromPhotonTeam(
                            PhotonNetwork.player.GetTeam()
                        )
                    );
                pv.RPC("ReceiveEveryoneChat", PhotonTargets.AllBuffered, username, message, teamTypeInt);
            }
        }
    }

    [PunRPC]
    private void ReceivePrivateTeamChat(string username, string message, int teamTypeInt)
    {
        if (PhotonNetwork.connected)
        {

        }
    }

    [PunRPC]
    private void ReceiveEveryoneChat(string username, string message, int teamTypeInt)
    {
        if (PhotonNetwork.connected)
        {

        }
    }
}
