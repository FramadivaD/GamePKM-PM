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
    }

    public void SelectItemOnIndex(int id)
    {
        Debug.Log("Try select item on index : " + id);
        if (inventory[id] != null)
        {
            player.EquipItem(inventory[id]);
            RefreshImageUI();

            inventorySelectedItemCursor.enabled = true;
            inventorySelectedItemCursor.rectTransform.position = inventoryButton[id].image.rectTransform.position;

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
        inventorySelectedItemCursor.enabled = false;

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
}
