using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInventoryAble
{

}

public class InventoryManager : MonoBehaviour
{
    private Player player;

    [SerializeField] private int maximumSlot = 2;
    public IInventoryAble[] inventory { get; private set; }

    [SerializeField] private List<Button> inventoryButton;
    [SerializeField] private List<Sprite> inventoryImageSprite;

    [SerializeField] private Image inventorySelectedItemCursor;
    [SerializeField] private Button dropButton;

    [SerializeField] private List<GameObject> inventoryOrbPrefabs;

    public bool IsFull
    {
        get
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void Initialize(Player player)
    {
        this.player = player;

        inventory = new IInventoryAble[maximumSlot];

        for (int i = 0; i < inventoryButton.Count; i++)
        {
            int id = i;
            Debug.Log("Register Slot : " + id);
            inventoryButton[i].onClick.AddListener(() => { SelectItemOnIndex(id); });
        }

        RefreshImageUI();
        RefreshSelectionCursor(-1);
    }

    public void SelectItemOnIndex(int id)
    {
        Debug.Log("Try select item on index : " + id);
        if (inventory[id] != null)
        {
            player.EquipItem(inventory[id]);
            RefreshImageUI();
            RefreshSelectionCursor(id);

            Debug.Log("Item selected");
        } else
        {
            Debug.Log("No item in this slot");
        }
    }

    public IInventoryAble DropItem(IInventoryAble item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
            {
                IInventoryAble temp = inventory[i];
                inventory[i] = null;

                RefreshImageUI();
                RefreshSelectionCursor(-1);

                SpawnDroppedItem(item);

                return temp;
            }
        }
        return null;
    }

    public bool AddItem(IInventoryAble item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;

                RefreshImageUI();

                return true;
            }
        }

        return false;
    }

    public IInventoryAble RemoveItem(IInventoryAble item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
            {
                inventory[i] = null;

                RefreshImageUI();

                return item;
            }
        }
        return null;
    }

    private void RefreshImageUI()
    {
        for (int i = 0; i < inventoryButton.Count && i < inventory.Length; i++)
        {
            Sprite image = GetImageSprite(inventory[i]);
            if (inventory[i] != null) {
                inventoryButton[i].interactable = true;
                inventoryButton[i].image.enabled = true;
                inventoryButton[i].image.sprite = image;
            } else {
                inventoryButton[i].interactable = false;
                inventoryButton[i].image.enabled = false;
            }
        }
    }

    private Sprite GetImageSprite(IInventoryAble item)
    {
        if (item != null) {
            if (item is MainGateFragment)
            {
                return inventoryImageSprite[0];
            }
            else if (item is WeaponInventory)
            {
                return inventoryImageSprite[1];
            }
        }
        return null;
    }

    private void RefreshSelectionCursor(int id)
    {
        if (id >= 0 && id < inventory.Length)
        {
            if (inventory[id] != null) {
                inventorySelectedItemCursor.enabled = true;
                dropButton.interactable = true;
                inventorySelectedItemCursor.rectTransform.position = inventoryButton[id].image.rectTransform.position;
            } else
            {
                inventorySelectedItemCursor.enabled = false;
                dropButton.interactable = false;
            }
        } else
        {
            inventorySelectedItemCursor.enabled = false;
            dropButton.interactable = false;
        }
    }

    private void SpawnDroppedItem(IInventoryAble item)
    {
        if (item != null) {
            if (item is MainGateFragment)
            {
                MainGateFragment fragment = item as MainGateFragment;
                
                GameObject ne;

                if (PhotonNetwork.connected)
                {
                    player.pv.RPC("SpawnFragmentOrbMaster", PhotonTargets.MasterClient, player.transform.position, (int)fragment.Team, fragment.FragmentIndex);
                } else
                {
                    ne = Instantiate(inventoryOrbPrefabs[0], player.transform.position, Quaternion.identity);
                    MainGateFragmentOrb orb = ne.GetComponent<MainGateFragmentOrb>();
                    orb.Initialize(fragment.Team, fragment.FragmentIndex);
                }
            } else if (item is WeaponInventory)
            {
                WeaponInventory weapon = item as WeaponInventory;
                if (weapon.weaponType == WeaponType.Basoka)
                {
                    GameObject ne;
                    if (PhotonNetwork.connected)
                    {
                        player.pv.RPC("SpawnWeaponOrbMaster", PhotonTargets.MasterClient, player.transform.position, (int)weapon.weaponType);
                    }
                    else
                    {
                        ne = Instantiate(inventoryOrbPrefabs[1], player.transform.position, Quaternion.identity);
                        WeaponOrb orb = ne.GetComponent<WeaponOrb>();
                        orb.Initialize((int)weapon.weaponType);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void SpawnFragmentOrbMaster(Vector3 pos, int teamType, int fragmentIndex)
    {
        GameObject ne = PhotonNetwork.Instantiate(inventoryOrbPrefabs[0].name, pos, Quaternion.identity, 0);
        MainGateFragmentOrb orb = ne.GetComponent<MainGateFragmentOrb>();
        orb.Initialize((TeamType)teamType, fragmentIndex);
    }

    [PunRPC]
    private void SpawnWeaponOrbMaster(Vector3 pos, int weaponType)
    {
        GameObject ne = PhotonNetwork.Instantiate(inventoryOrbPrefabs[1].name, pos, Quaternion.identity, 0);

        WeaponOrb orb = ne.GetComponent<WeaponOrb>();
        orb.Initialize(weaponType);
    }
}
