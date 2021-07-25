using UnityEngine;
using System.Collections;

public class LobbyTeacherRoomQuestionDifficulty : MonoBehaviour
{
    [SerializeField] private GameObject difficultyMenu;

    public string SelectedDifficulty { get; private set; }

    public void OpenDifficultyMenu()
    {
        difficultyMenu.SetActive(true);
    }

    public void CloseDifficultyMenu()
    {
        difficultyMenu.SetActive(false);
    }
}
