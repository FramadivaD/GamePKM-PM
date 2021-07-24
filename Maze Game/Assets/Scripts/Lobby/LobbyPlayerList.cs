using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyPlayerList : MonoBehaviour
{
    [SerializeField] private string[] playersJoinedID;
    [SerializeField] private string[] playersJoinedName;

    [SerializeField] private LobbyPlayerPreview[] playersPreviewList;

    public void RefreshPreview()
    {
        for (int i = 0; i < 8;i++)
        {
            playersPreviewList[i].Initialize(playersJoinedID[i], playersJoinedName[i]);
        }
    }
}
