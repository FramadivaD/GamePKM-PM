using UnityEngine;

public class MainGateFragmentOrb : MonoBehaviour
{
    [SerializeField] private MainGateFragment fragment;

    public MainGateFragment Fragment { get { return fragment; } }

    public void Initialize(MainGateFragment fragment)
    {
        this.fragment = fragment;
    }

    public MainGateFragment TakeFragment(Player player)
    {
        if (!player.inventoryManager.IsFull)
        {
            player.inventoryManager.AddItem(Fragment);
            Destroy(gameObject);
            return fragment;
        }
        return null;
    }
}
