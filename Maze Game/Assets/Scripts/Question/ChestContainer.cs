using UnityEngine;
using System.Collections;

public class ChestContainer : MonoBehaviour
{
    public Question question;
    public TeamType teamType;
    public MainGateFragment fragmentKey;

    public GameObject unlockedGraphic;
    public GameObject lockedGraphic;

    public SpriteRenderer teamIndicator;

    private bool isUnlocked = false;
    public bool IsUnlocked {
        get {
            return isUnlocked;
        }
        private set {
            isUnlocked = value;
            unlockedGraphic.SetActive(isUnlocked);
            lockedGraphic.SetActive(!isUnlocked);
            teamIndicator.color = TeamHelper.GetColorTeam(teamType);
        }
    }

    public bool IsFragmentTaken { get; private set; }

    public void Initialize()
    {
        IsUnlocked = false;
        IsFragmentTaken = false;

        this.question = new Question();
    }

    public void Initialize(TeamType teamType, MainGateFragment fragment)
    {
        Initialize();
        this.teamType = teamType;
        this.fragmentKey = fragment;
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

    public MainGateFragment TryTakeFragmentKey(Player player)
    {
        if (!IsFragmentTaken)
        {
            if (fragmentKey != null)
            {
                if (player.teamType == teamType)
                {
                    // check inventory dulu harunya
                    player.inventoryManager.AddItem(fragmentKey);
                    fragmentKey = null;

                    IsFragmentTaken = true;

                    Debug.Log("Fragment Key saved in inventory.");
                }
                else
                {
                    Debug.Log("Must be taken by other team.");
                }
            }
        } else
        {
            Debug.Log("Fragment key has been taken.");
        }
        return null;
    }
}
