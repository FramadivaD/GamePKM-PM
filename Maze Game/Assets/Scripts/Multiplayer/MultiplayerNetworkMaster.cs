using UnityEngine;
using System.Collections;

public class MultiplayerNetworkMaster : Photon.PunBehaviour
{
    public PhotonView pv;

    public QuestionDifficulty questionDifficulty;
    public MainGateKeyRaw mainGateRaw;

    private void Start()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            string diffJson = LobbyTeacherRoomQuestionDifficulty.SelectedDifficultyJson;
            string keyJson = LobbyTeacherRoomMainGate.CurrentMainGateKeyJson;

            Debug.Log("Karena master jadi akan ngirim data");

            pv.RPC("ReceiveMainGateKeyAndQuestionDifficultyData", PhotonTargets.AllBuffered, diffJson, keyJson);
        }
    }

    [PunRPC]
    private void ReceiveMainGateKeyAndQuestionDifficultyData(string diffJson, string keyJson)
    {
        Debug.Log("Meskipun master / client akan menerima data");

        questionDifficulty = JsonUtility.FromJson<QuestionDifficulty>(diffJson);
        mainGateRaw = JsonUtility.FromJson<MainGateKeyRaw>(keyJson);

        LoadMainGateKeyAndQuestionDifficultyData(questionDifficulty, mainGateRaw);

        if (PhotonNetwork.player.IsMasterClient)
        {
            Debug.Log("Karena master maka akan main sebagai spectator");

            PlayAsSpectator();
        } else 
        {
            Debug.Log("Karena client maka akan main sebagai player");

            PlayAsPlayer();
        }
    }

    private void LoadMainGateKeyAndQuestionDifficultyData(QuestionDifficulty questionDifficulty, MainGateKeyRaw mainGateRaw)
    {
        TeamType team = TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam());
        GameManager.Instance.LoadFragmentsKey(MainGateKey.ConvertFromRawData(team, mainGateRaw));

        GameManager.Instance.LoadQuestionDifficulty(questionDifficulty);
    }

    private void PlayAsPlayer()
    {
        TeamType team = TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam());
        GameManager.Instance.BeginPlayer(team);
    }

    private void PlayAsSpectator()
    {
        GameManager.Instance.BeginSpectator();
    }
}
