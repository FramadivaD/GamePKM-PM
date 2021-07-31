using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomTeamChat : MonoBehaviour
{
    public PhotonView pv;

    [SerializeField] private RoomTeamChatText roomChatTextPrefab;

    [SerializeField] private GameObject chatPanel;

    [SerializeField] private GameObject everyoneChatPanel;
    [SerializeField] private GameObject privateTeamChatPanel;

    [SerializeField] private Transform everyoneChatContainer;
    [SerializeField] private Transform privateTeamChatContainer;

    private bool receivePrivateTeamChat = false;

    private void Start()
    {
        
    }

    public void Initialize()
    {
        receivePrivateTeamChat = false;

        everyoneChatPanel.gameObject.SetActive(false);
        privateTeamChatPanel.gameObject.SetActive(false);

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
        everyoneChatPanel.gameObject.SetActive(true);

        foreach(Transform t in everyoneChatContainer)
        {
            Destroy(t.gameObject);
        }
    }

    private void InitializePrivateTeamChat()
    {
        privateTeamChatPanel.gameObject.SetActive(true);

        foreach (Transform t in privateTeamChatContainer)
        {
            Destroy(t.gameObject);
        }

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
            TeamType teamType = (TeamType)teamTypeInt;
            TeamType playerTeamType = TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam());

            // kalo sama maka receive
            if (teamType == playerTeamType)
            {
                RoomTeamChatText text = Instantiate(roomChatTextPrefab.gameObject, privateTeamChatContainer).GetComponent<RoomTeamChatText>();
                text.Initialize(username, message, teamTypeInt);
            }
        }
    }

    [PunRPC]
    private void ReceiveEveryoneChat(string username, string message, int teamTypeInt)
    {
        if (PhotonNetwork.connected)
        {
            TeamType teamType = (TeamType)teamTypeInt;

            // semua bakal receive
            RoomTeamChatText text = Instantiate(roomChatTextPrefab.gameObject, everyoneChatContainer).GetComponent<RoomTeamChatText>();
            text.Initialize(username, message, teamTypeInt);
        }
    }

    public void OpenChatPanel()
    {
        chatPanel.SetActive(true);
    }

    public void HideChatPanel()
    {
        chatPanel.SetActive(false);
    }

    public void OpenEveryoneChatPanel()
    {
        everyoneChatPanel.SetActive(true);
        privateTeamChatPanel.SetActive(false);
    }

    public void OpenPrivateChatPanel()
    {
        everyoneChatPanel.SetActive(false);
        privateTeamChatPanel.SetActive(true);
    }
}
