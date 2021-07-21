using UnityEngine;
using System.Collections;

public class ChestContainer : MonoBehaviour
{
    public Question question;
    public TeamType teamType;

    public bool IsUnlocked { get; private set; }

    public void Initialize(TeamType teamType)
    {
        this.question = new Question();
        this.teamType = teamType;
    }

    public bool CheckAnswer(string answer)
    {
        return answer == question.question;
    }

    public void UnlockChest()
    {
        IsUnlocked = true;
    }
}
