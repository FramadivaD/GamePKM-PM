using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Extensione.Window;

public class LobbyTeacherRoom : Photon.PunBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private LobbyTeacherRoomMainGate mainGateMenu;
    [SerializeField] private LobbyTeacherRoomQuestionDifficulty questionDifficultyMenu;

    [SerializeField] private LobbyPlayerList playerList;

    [SerializeField] private FirebaseManager firebaseManager;

    [SerializeField] private bool skipMustAtLeastAPlayer = false;

    [SerializeField] private Button selectSoalButton;
    [SerializeField] private Button selectDiffButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button leaveGameButton;

    [SerializeField] private Button selectRedTeamButton;
    [SerializeField] private Button selectBlueTeamButton;

    public static string MainGateDownloadURL;

    public void CheckStartGame()
    {
        if (PhotonNetwork.player.IsMasterClient) {
            if (LobbyTeacherRoomMainGate.CurrentMainGateKey != null)
            {
                if (!LobbyTeacherRoomQuestionDifficulty.SelectedDifficulty.CheckAllInactive())
                {
                    int redTeam = GetRedTeamCount();
                    int blueTeam = GetBlueTeamCount();

                    // Change this to true to debug mode
                    skipMustAtLeastAPlayer = false;

                    // if (!skipMustAtLeastAPlayer && (redTeam <= 0 || blueTeam <= 0))
                    if (!skipMustAtLeastAPlayer && (redTeam <= 0 && blueTeam <= 0))
                    {
                        Debug.Log("Each team must have at least 1 player.");

                        WindowMaster.Instance.Show("Setidaknya harus terdapat 1 pemain di masing - masing team.");
                    }
                    else
                    {
                        UploadMainGateKeyFile();
                    }
                } else
                {
                    Debug.Log("At least must has 1 difficulty.");

                    WindowMaster.Instance.Show("Setidaknya harus terdapat 1 jenis soal!");
                }
            } else
            {
                Debug.Log("Teacher Must select 1 Question for Main Gate");

                WindowMaster.Instance.Show("Harus memilih Soal untuk Main Gate!");
            }
        }
    }

    private void UploadMainGateKeyFile()
    {
        WindowMaster.Instance.Show("Uploading Soal..");

        SendUploadMessageToClient();

        string filename = SystemInfo.deviceUniqueIdentifier + "/" + AndroidHelper.Base64Encode(LobbyTeacherRoomMainGate.CurrentMainGateKey.GateName);
        string data = LobbyTeacherRoomMainGate.CurrentMainGateKeyJson;
        firebaseManager.UploadData(filename, System.Text.Encoding.ASCII.GetBytes(data),
            () => {
                Debug.Log("Upload Success. So the Game will starting.");

                WindowMaster.Instance.Show("Upload Soal berhasil.\nMemulai Game!");

                MainGateDownloadURL = filename;
                MasterStartGame();
            },
            () => {
                Debug.Log("Upload failed. So the Game not starting.");

                WindowMaster.Instance.Show("Upload Soal gagal. Coba mulai kembali.");
                MasterAbortGame();
            }
            );
    }

    private void SendUploadMessageToClient()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                LockAllButtons();

                pv.RPC("SendUploadMessageToClientRPC", PhotonTargets.OthersBuffered);
            }
        } else
        {
            SendUploadMessageToClientRPC();
        }
    }

    [PunRPC]
    private void SendUploadMessageToClientRPC()
    {
        WindowMaster.Instance.Show("Memulai Game..");

        LockAllButtons();
    }

    private void MasterAbortGame()
    {
        // Unlock Room
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                PhotonNetwork.room.IsOpen = true;
                PhotonNetwork.room.IsVisible = true;
            }
        }

        if (PhotonNetwork.player.IsMasterClient)
        {
            pv.RPC("ClientAbortGame", PhotonTargets.Others);

            WindowMaster.Instance.Show("Upload soal gagal. Game dibatalkan.");

            UnlockAllButtons();
        }
    }

    [PunRPC]
    private void ClientAbortGame()
    {
        WindowMaster.Instance.Show("Master gagal memulai Game.");

        UnlockAllButtons();
    }

    private void MasterStartGame()
    {
        // Lock Room
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
            }
        }

        if (PhotonNetwork.player.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameplayScene");

            pv.RPC("ClientJoinGame", PhotonTargets.Others);
        }
    }

    [PunRPC]
    private void ClientJoinGame()
    {
        WindowMaster.Instance.Show("Memulai Game..");

        if (!PhotonNetwork.player.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameplayScene");
        }
    }

    private int GetRedTeamCount()
    {
        int redTeam = 0;
        for (int i = 0; i < 4; i++)
        {
            if (playerList.Players.players[i] != null && playerList.Players.players[i].playerExist)
            {
                redTeam++;
            }
        }
        return redTeam;
    }

    private int GetBlueTeamCount()
    {
        int blueTeam = 0;

        for (int i = 4; i < 8; i++)
        {
            if (playerList.Players.players[i] != null && playerList.Players.players[i].playerExist)
            {
                blueTeam++;
            }
        }
        return blueTeam;
    }

    public void LockAllButtons()
    {
        selectSoalButton.interactable = false;
        selectDiffButton.interactable = false;
        startGameButton.interactable = false;
        leaveGameButton.interactable = false;

        selectRedTeamButton.interactable = false;
        selectBlueTeamButton.interactable = false;
    }

    public void UnlockAllButtons()
    {
        selectSoalButton.interactable = true;
        selectDiffButton.interactable = true;
        startGameButton.interactable = true;
        leaveGameButton.interactable = true;

        selectRedTeamButton.interactable = true;
        selectBlueTeamButton.interactable = true;
    }
}
