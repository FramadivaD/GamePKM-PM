using UnityEngine;
using System.Collections;

public class LobbyTeacherRoom : Photon.PunBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private LobbyTeacherRoomMainGate mainGateMenu;
    [SerializeField] private LobbyTeacherRoomQuestionDifficulty questionDifficultyMenu;

    [SerializeField] private LobbyPlayerList playerList;

    public void CheckStartGame()
    {
        if (PhotonNetwork.player.IsMasterClient) {
            if (mainGateMenu.CurrentMainGateKey != null)
            {
                int redTeam = GetRedTeamCount();
                int blueTeam = GetRedTeamCount();

                if (redTeam <= 0 || blueTeam <= 0)
                {
                    Debug.Log("Each team must have at least 1 player.");
                }
                else
                {
                    StartGame();
                }
            } else
            {
                Debug.Log("Teacher Must select 1 Question for Main Gate");
            }
        }
    }

    public void StartGame()
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
