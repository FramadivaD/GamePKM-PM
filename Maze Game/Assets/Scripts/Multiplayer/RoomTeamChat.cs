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

    [SerializeField] private InputField chatInputText;
    [SerializeField] private Button chatPanelButton;

    [SerializeField] private Button chatPrivateTeamPanelButton;
    [SerializeField] private Button chatEveryonePanelButton;

    [SerializeField] private ScrollRect chatScroll;
    [SerializeField] private ContentSizeFitter chatContentFitter;
    [SerializeField] private GridLayoutGroup chatGridLayout;

    private Vector3 panelOriginalPosition;
    private bool chatPanelIsHiding = false;

    private void Awake()
    {
        panelOriginalPosition = chatPanel.transform.localPosition;
    }

    private void Start()
    {
        HideChatPanel();
    }

    public void Initialize()
    {
        receivePrivateTeamChat = false;

        if (PhotonNetwork.connected)
        {
            InitializeTeamChat();

            if (PhotonNetwork.player.IsMasterClient)
            {
                chatPrivateTeamPanelButton.gameObject.SetActive(false);
            } else
            {
                chatPrivateTeamPanelButton.gameObject.SetActive(true);
            }
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
        if (chatInputText.text.Length > 0)
        {
            if (sendPrivateTeamChat)
            {
                SendPrivateTeamChat(chatInputText.text);
            }
            else
            {
                SendEveryoneChat(chatInputText.text);
            }

            chatInputText.text = "";
        }
    }

    public void SendPrivateTeamChat(string message)
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
            if (!PhotonNetwork.player.IsMasterClient)
            {
                TeamType teamType = (TeamType)teamTypeInt;
                TeamType playerTeamType = 
                    (TeamType) TeamHelper.GetColorTeamAlterIndex(
                        TeamHelper.FromPhotonTeam(
                            PhotonNetwork.player.GetTeam()
                            )
                        );

                // kalo sama maka receive
                if (teamType == playerTeamType)
                {
                    RoomTeamChatText text = Instantiate(roomChatTextPrefab.gameObject, textChatContainer).GetComponent<RoomTeamChatText>();
                    text.Initialize(true, username, message, teamTypeInt);

                    chatGridLayout.CalculateLayoutInputVertical();
                    chatContentFitter.SetLayoutVertical();
                    chatScroll.verticalNormalizedPosition = 0;
                }
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

            Canvas.ForceUpdateCanvases();

            chatGridLayout.CalculateLayoutInputVertical();
            chatContentFitter.SetLayoutVertical();
            chatScroll.verticalNormalizedPosition = 0;
        }
    }

    private void CheckChatIsMaster()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            chatPrivateTeamPanelButton.gameObject.SetActive(false);
        }
        else
        {
            chatPrivateTeamPanelButton.gameObject.SetActive(true);
        }
    }

    public void OpenChatPanel()
    {
        // chatPanel.SetActive(true);
        chatPanelIsHiding = false;
        chatPanel.transform.localPosition = panelOriginalPosition;

        chatPanelButton.transform.localScale = new Vector3(1, 1, 1);

        CheckChatIsMaster();
        RefreshChatTypePrep();
    }

    public void HideChatPanel()
    {
        // chatPanel.SetActive(false);
        chatPanelIsHiding = true;
        chatPanel.transform.localPosition = new Vector3(10000, 10000, 10000);

        chatPanelButton.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void ToggleChatPanel()
    {
        //if (chatPanel.activeSelf)
        if (!chatPanelIsHiding)
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

        RefreshChatTypePrep();
    }

    public void PreparePrivateTeamChat()
    {
        sendPrivateTeamChat = true;

        RefreshChatTypePrep();
    }

    private void RefreshChatTypePrep()
    {
        if (sendPrivateTeamChat)
        {
            chatPrivateTeamPanelButton.image.color = new Color(1, 1, 1, 1);
            chatEveryonePanelButton.image.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            chatPrivateTeamPanelButton.image.color = new Color(1, 1, 1, 0.5f);
            chatEveryonePanelButton.image.color = new Color(1, 1, 1, 1);
        }
    }
}
