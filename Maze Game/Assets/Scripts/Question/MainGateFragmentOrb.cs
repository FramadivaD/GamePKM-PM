using UnityEngine;
using UnityEngine.UI;

using Extensione.Audio;

public class MainGateFragmentOrb : MonoBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private MainGateFragment fragment;
    public MainGateFragment Fragment { get { return fragment; } }

    [Header("UI")]
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private Image fragmentImage;
    [SerializeField] private Text fragmentDataText;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSFX;

    public void Initialize(TeamType teamType, int fragmentIndex)
    {
        if (PhotonNetwork.connected)
        {
            pv.RPC("InitializeRPC", PhotonTargets.AllBuffered, (int)teamType, fragmentIndex);
        } else
        {
            InitializeRPC((int)teamType, fragmentIndex);
        }
    }

    [PunRPC]
    private void InitializeRPC(int team, int fragmentIndex)
    {
        TeamType teamType = (TeamType)team;

        this.fragment = GameManager.PlayersTeam[teamType].FragmentsKey.Fragments[fragmentIndex];

        canvasUI.SetActive(true);

        Sprite sprite = fragment.DataImage;

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
        fragmentDataText.text = fragment.Data;
    }

    public MainGateFragment TakeFragment(Player player)
    {
        // check if team is equal
        if (player.teamType == fragment.Team)
        {
            if (!player.inventoryManager.IsFull)
            {
                canvasUI.SetActive(false);
                player.inventoryManager.AddItem(Fragment);

                AudioManager.Instance.PlaySFXOnce(pickupSFX);

                if (PhotonNetwork.connected)
                {
                    pv.RPC("DestroyOrb", PhotonTargets.MasterClient);
                }
                else
                {
                    Destroy(gameObject);
                }

                return fragment;
            }
        }
        return null;
    }

    [PunRPC]
    private void DestroyOrb()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
