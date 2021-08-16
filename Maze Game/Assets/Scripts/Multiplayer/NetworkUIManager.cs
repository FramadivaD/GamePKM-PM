using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkUIManager : MonoBehaviour
{
    public MultiplayerNetworkMaster networkMaster;

    public GameObject networkUIWindow;

    public Text masterNotifText;
    public Text clientsNotifText;

    public Button masterButton;

    private void Start()
    {
        masterButton.gameObject.SetActive(false);
        ShowWindow();
    }

    public void MasterCountPlayer(int count, int max)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                networkMaster.pv.RPC("MasterCountPlayerRPC", PhotonTargets.AllBuffered, count, max);
            }
        } else
        {
            MasterCountPlayerRPC(count, max);
        }
    }

    [PunRPC]
    private void MasterCountPlayerRPC(int count, int max)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                masterNotifText.text = $"Waiting player.. ({count}/{max})";
            }
            else
            {
                clientsNotifText.text = $"Waiting player.. ({count}/{max})";
            }
        }
        else
        {
            masterNotifText.text = $"Waiting player.. ({count}/{max})";
        }
    }

    public void MasterSendMainGateData()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                masterNotifText.text = "Sending question data..";
            }
        } else
        {
            masterNotifText.text = "Sending question data..";
        }
    }

    public void DownloadMainGateData()
    {
        masterNotifText.text = "Retrieving question data..";
        clientsNotifText.text = "Retrieving question data..";
    }

    public void DownloadMainGateDataSuccess()
    {
        masterNotifText.text = "Question data retrieved successfully!";
        clientsNotifText.text = "Question data retrieved successfully!";
    }

    public void DownloadMainGateDataFailed()
    {
        masterNotifText.text = "Retrieve question data failed!\nReturn to Lobby Menu.";
        clientsNotifText.text = "Retrieve question data failed!\nReturn to Lobby Menu";
    }

    public void MasterWaitToStartTheGame()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                masterNotifText.text = "Ready to start the game!";

                networkMaster.pv.RPC("ClientWaitMaster", PhotonTargets.OthersBuffered);
            }
        } else
        {
            masterNotifText.text = "Ready to start the game!";
        }
    }

    public void MasterShowStartGameButton()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                masterButton.gameObject.SetActive(true);
            }
        } else
        {
            masterButton.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void ClientWaitMaster()
    {
        if (PhotonNetwork.connected) {
            if (!PhotonNetwork.player.IsMasterClient) {
                clientsNotifText.text = "Waiting teacher to start the game..";
            }
        } else
        {
            clientsNotifText.text = "Waiting teacher to start the game..";
        }
    }

    public void StartAsMaster()
    {
        ShowWindow();

        clientsNotifText.gameObject.SetActive(false);
        masterNotifText.gameObject.SetActive(true);
    }

    public void StartAsClient()
    {
        ShowWindow();

        clientsNotifText.gameObject.SetActive(true);
        masterNotifText.gameObject.SetActive(false);
    }

    public void HideWindow()
    {
        if (PhotonNetwork.connected)
        {
            networkMaster.pv.RPC("HideWindowRPC", PhotonTargets.AllBuffered);
        } else
        {
            HideWindowRPC();
        }
    }

    [PunRPC]
    private void HideWindowRPC()
    {
        networkUIWindow.SetActive(false);
    }

    public void ShowWindow()
    {
        networkUIWindow.SetActive(true);
    }
}
