using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomTeamChat : MonoBehaviour
{
    public PhotonView pv;

    [SerializeField] private RoomTeamChatText roomChatTextPrefab;

    [SerializeField] private GameObject chatPanel;

    [SerializeField] private Transform textChatContainer;

    private bool receivePrivateTeamChat = false;

    private bool sendPrivateTeamChat = false;

    public InputField chatInputText;

    public void Initialize()
    {
        receivePrivateTeamChat = false;

        if (PhotonNetwork.connected)
        {
            InitializeTeamChat();
        }
    }

    private void InitializeTeamChat()
    {
        foreach (Transform t in textChatContainer)
        {
            Destroy(t.gameObject);
        }

        receivePrivateTeamChat = true;
    }

    public void SendChat()
    {
        if (sendPrivateTeamChat)
        {
            SendPrivateTeamChat(chatInputText.text);
        } else
        {
            SendEveryoneChat(chatInputText.text);
        }
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
                RoomTeamChatText text = Instantiate(roomChatTextPrefab.gameObject, textChatContainer).GetComponent<RoomTeamChatText>();
                text.Initialize(true, username, message, teamTypeInt);
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
            RoomTeamChatText text = Instantiate(roomChatTextPrefab.gameObject, textChatContainer).GetComponent<RoomTeamChatText>();
            text.Initialize(false, username, message, teamTypeInt);
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

    public void ToggleChatPanel()
    {
        if (chatPanel.activeSelf)
        {
            HideChatPanel();
        } else
        {
            OpenChatPanel();
        }
    }

    public void PrepareEveryoneChat()
    {
        sendPrivateTeamChat = false;
    }

    public void PreparePrivateTeamChat()
    {
        sendPrivateTeamChat = true;
    }
}
