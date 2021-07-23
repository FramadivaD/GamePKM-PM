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
        fragmentImage.sprite = fragment.DataImage;
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
