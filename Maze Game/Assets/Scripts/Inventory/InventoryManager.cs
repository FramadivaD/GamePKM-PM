using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryAble
{

}

public class InventoryManager : MonoBehaviour
{
    public int maximumSlot = 2;
    public IInventoryAble[] inventory;

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
}
