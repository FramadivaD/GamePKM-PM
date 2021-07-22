using UnityEngine;
using System.Collections;

public class WeaponOrb : MonoBehaviour
{
    [SerializeField] private WeaponInventory weapon;

    public WeaponInventory Weapon { get { return weapon; } }

    public void Initialize(WeaponInventory weapon)
    {
        this.weapon = weapon;
    }

    public WeaponInventory TakeWeapon(Player player)
    {
        if (!player.inventoryManager.IsFull)
        {
            player.inventoryManager.AddItem(Weapon);
            Destroy(gameObject);
            return weapon;
        }
        return null;
    }
}
