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
                masterNotifText.text = $"Menunggu player.. ({count}/{max})";
            }
            else
            {
                clientsNotifText.text = $"Menunggu player.. ({count}/{max})";
            }
        }
        else
        {
            masterNotifText.text = $"Menunggu player.. ({count}/{max})";
        }
    }

    public void MasterSendMainGateData()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                masterNotifText.text = "Mengirim data soal..";
            }
        } else
        {
            masterNotifText.text = "Mengirim data soal..";
        }
    }

    public void DownloadMainGateData()
    {
        masterNotifText.text = "Mengunduh data soal..";
        clientsNotifText.text = "Mengunduh data soal..";
    }

    public void DownloadMainGateDataSuccess()
    {
        masterNotifText.text = "Data soal berhasil diunduh!";
        clientsNotifText.text = "Data soal berhasil diunduh!";
    }

    public void DownloadMainGateDataFailed()
    {
        masterNotifText.text = "Data soal gagal diunduh!\nKembali ke main menu.";
        clientsNotifText.text = "Data soal gagal diunduh!\nKembali ke main menu.";
    }

    public void MasterWaitToStartTheGame()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                masterNotifText.text = "Siap untuk memulai game!";
            } else
            {
                networkMaster.pv.RPC("ClientWaitMaster", PhotonTargets.OthersBuffered);
            }
        } else
        {
            masterNotifText.text = "Siap untuk memulai game!";
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
                clientsNotifText.text = "Menunggu guru untuk memulai game..";
            }
        } else
        {
            clientsNotifText.text = "Menunggu guru untuk memulai game..";
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
