using UnityEngine;
using System.Collections;

public class LobbyNetwork : MonoBehaviour
{
    private string GameVersion { get { return Application.version; } }

    private void Awake()
    {
        InitializePhotonNetwork();
    }

    private void InitializePhotonNetwork()
    {
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

    private void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");
    }
}
