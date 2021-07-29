using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Extensione.Audio;

public class ChestContainer : MonoBehaviour
{
    [SerializeField] private PhotonView pv;

    public Question question;
    public TeamType teamType;
    public MainGateFragment fragmentKey;

    public GameObject unlockedGraphic;
    public GameObject lockedGraphic;

    public SpriteRenderer teamIndicator;

    [Header("UI")]
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private Image fragmentImage;
    [SerializeField] private Text fragmentDataText;

    [Header("Audio")]
    [SerializeField] private AudioClip unlockChestSFX;
    [SerializeField] private AudioClip takeFragmentSFX;

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

    public void Initialize(TeamType teamType, int fragmentIndex)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient) {
                pv.RPC("InitializeRPCAll", PhotonTargets.AllBuffered, (int)teamType, fragmentIndex);
            }
        } else
        {
            InitializeDirect(teamType, fragmentIndex);
        }
    }

    private void InitializeDirect(TeamType teamType, int fragmentIndex)
    {
        this.teamType = teamType;
        this.fragmentKey = GameManager.PlayersTeam[teamType].FragmentsKey.Fragments[fragmentIndex];

        Initialize();

        Sprite sprite = fragmentKey.DataImage;

        if (sprite != null)
        {
            int width = sprite.texture.width;
            int height = sprite.texture.height;

            float alterWidth = fragmentImage.rectTransform.sizeDelta.x;
            //float alterHeight = fragmentImage.rectTransform.sizeDelta.y * width / height;
            float alterHeight = fragmentImage.rectTransform.sizeDelta.x * height / width;

            fragmentImage.rectTransform.sizeDelta = new Vector2(alterWidth, alterHeight);
        }

        fragmentImage.sprite = sprite;
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
        if (PhotonNetwork.connected)
        {
            pv.RPC("UnlockChestRPCAll", PhotonTargets.AllBuffered);
        } else
        {
            UnlockChestRPCAll();
        }
    }

    [PunRPC]
    private void UnlockChestRPCAll()
    {
        AudioManager.Instance.PlaySFXOnce(unlockChestSFX);

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

                        if (PhotonNetwork.connected)
                        {
                            pv.RPC("TakeFragmentKeyRPCAll", PhotonTargets.AllBuffered);
                        } else
                        {
                            TakeFragmentKeyRPCAll();
                        }

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

    [PunRPC]
    private void InitializeRPCAll(int team, int fragmentIndex)
    {
        TeamType teamType = (TeamType)team;

        InitializeDirect(teamType, fragmentIndex);
    }

    [PunRPC]
    private void TakeFragmentKeyRPCAll()
    {
        fragmentKey = null;

        IsFragmentTaken = true;

        canvasUI.SetActive(false);

        AudioManager.Instance.PlaySFXOnce(takeFragmentSFX);
    }
}
