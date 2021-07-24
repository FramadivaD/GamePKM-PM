using UnityEngine;
using UnityEngine.UI;

public class MainGateFragmentOrb : MonoBehaviour
{
    [SerializeField] private MainGateFragment fragment;

    public MainGateFragment Fragment { get { return fragment; } }

    [Header("UI")]
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private Image fragmentImage;
    [SerializeField] private Text fragmentDataText;

    public void Initialize(MainGateFragment fragment)
    {
        this.fragment = fragment;

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
        if (!player.inventoryManager.IsFull)
        {
            canvasUI.SetActive(false);
            player.inventoryManager.AddItem(Fragment);
            Destroy(gameObject);
            return fragment;
        }
        return null;
    }
}
