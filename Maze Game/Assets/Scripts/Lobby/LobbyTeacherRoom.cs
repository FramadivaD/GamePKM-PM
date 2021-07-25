using UnityEngine;
using System.Collections;

public class LobbyTeacherRoom : Photon.PunBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private LobbyTeacherRoomMainGate mainGateMenu;
    [SerializeField] private LobbyTeacherRoomQuestionDifficulty questionDifficultyMenu;

    [SerializeField] private LobbyPlayerList playerList;

    [SerializeField] private FirebaseManager firebaseManager;

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

                    bool skip = true;

                    if (!skip && (redTeam <= 0 || blueTeam <= 0))
                    {
                        Debug.Log("Each team must have at least 1 player.");
                    }
                    else
                    {
                        //StartGame();
                        UploadMainGateKeyFile();
                    }
                } else
                {
                    Debug.Log("At least must has 1 difficulty.");
                }
            } else
            {
                Debug.Log("Teacher Must select 1 Question for Main Gate");
            }
        }
    }

    private void UploadMainGateKeyFile()
    {
        string filename = SystemInfo.deviceUniqueIdentifier + "/" + AndroidHelper.Base64Encode(LobbyTeacherRoomMainGate.CurrentMainGateKey.GateName);
        string data = LobbyTeacherRoomMainGate.CurrentMainGateKeyJson;
        firebaseManager.UploadData(filename, System.Text.Encoding.ASCII.GetBytes(data),
            () => {
                Debug.Log("Upload Success. So the Game will starting.");
                MainGateDownloadURL = filename;
                StartGame();
            },
            () => {
                Debug.Log("Upload failed. So the Game not starting.");
            }
            );
    }

    private void StartGame()
    {
        // pv.RPC();
        PhotonNetwork.LoadLevel("GameplayScene");
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
}
