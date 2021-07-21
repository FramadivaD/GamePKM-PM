using UnityEngine;
using System.Collections;

public class ChestContainer : MonoBehaviour
{
    public Question question;
    public TeamType teamType;

    public GameObject unlockedGraphic;
    public GameObject lockedGraphic;

    private bool isUnlocked = false;
    public bool IsUnlocked {
        get {
            return isUnlocked;
        }
        private set {
            isUnlocked = value;
            unlockedGraphic.SetActive(isUnlocked);
            lockedGraphic.SetActive(!isUnlocked);
        }
    }

    private void Start()
    {
        Initialize(teamType);
    }

    public void Initialize()
    {
        IsUnlocked = false;

        this.question = new Question();
    }

    public void Initialize(TeamType teamType)
    {
        Initialize();
        this.teamType = teamType;
    }

    public bool CheckAnswer(string answer)
    {
        if (question == null)
        {
            Debug.LogWarning("Chest Opened, because there is no question.");
            return true;
        }
        return answer == question.answer;
    }

    public void UnlockChest()
    {
        IsUnlocked = true;
    }
}
