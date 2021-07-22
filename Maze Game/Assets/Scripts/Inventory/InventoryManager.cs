using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInventoryAble
{

}

public class InventoryManager : MonoBehaviour
{
    public int maximumSlot = 2;
    public IInventoryAble[] inventory { get; private set; }

    public List<Image> inventoryImage;
    public List<Sprite> inventoryImageSprite;

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

    public InventoryManager()
    {
        inventory = new IInventoryAble[maximumSlot];
    }

    public bool AddItem(IInventoryAble item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                return true;
            }
        }

        //full
        return false;
    }

    public IInventoryAble RemoveItem(IInventoryAble item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
            {
                inventory[i] = null;
                return item;
            }
        }
        return null;
    }

    private void RefreshImageUI()
    {
        for (int i = 0; i < inventoryImage.Count && i < inventory.Length; i++)
        {
            inventoryImage[i].sprite = GetImageSprite(inventory[i]);
        }
    }

    private Sprite GetImageSprite(IInventoryAble item)
    {
        if (item != null) {
            if (item is MainGateFragment)
            {
                return inventoryImageSprite[0];
            }
        }
        return null;
    }
}
