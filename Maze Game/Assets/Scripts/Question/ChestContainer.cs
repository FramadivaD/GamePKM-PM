using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChestContainer : MonoBehaviour
{
    public Question question;
    public TeamType teamType;
    public MainGateFragment fragmentKey;

    public GameObject unlockedGraphic;
    public GameObject lockedGraphic;

    public SpriteRenderer teamIndicator;

    [Header("UI")]
    public GameObject canvasUI;
    public Image fragmentImage;
    public Text fragmentDataText;

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

            canvasUI.SetActive(isUnlocked && !IsFragmentTaken);
        }
    }

    public bool IsFragmentTaken { get; private set; }

    public void Initialize()
    {
        IsUnlocked = false;
        IsFragmentTaken = false;

        this.question = new Question();

        canvasUI.SetActive(false);
    }

    public void Initialize(TeamType teamType, MainGateFragment fragment)
    {
        Initialize();
        this.teamType = teamType;
        this.fragmentKey = fragment;

        fragmentImage.sprite = null;
        fragmentDataText.text = fragmentKey.Data;
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
                    // check inventory dulu, kalo penuh jangan di buang
                    if (!player.inventoryManager.IsFull)
                    {
                        player.inventoryManager.AddItem(fragmentKey);
                        fragmentKey = null;

                        IsFragmentTaken = true;

                        canvasUI.SetActive(false);

                        Debug.Log("Fragment Key saved in inventory.");
                    } else
                    {
                        Debug.Log("Inventory is FULL");
                    }
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
