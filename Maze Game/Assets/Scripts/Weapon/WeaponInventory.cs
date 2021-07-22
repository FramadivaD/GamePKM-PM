using UnityEngine;
using System.Collections;

public enum WeaponType
{
    None,
    Basoka,
    Panah,
    Pedang
}

public class WeaponInventory :  IInventoryAble
{
    public WeaponType weaponType;
}
