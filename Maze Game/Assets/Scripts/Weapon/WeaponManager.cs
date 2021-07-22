using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    Player player;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private SpriteRenderer weaponGraphic;
    [SerializeField] private List<Sprite> weaponSprites;

    [SerializeField] private Transform weaponControl;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void ExecuteWeapon(WeaponInventory weapon)
    {
        if (weapon != null)
        {
            if (weapon.weaponType == WeaponType.None)
            {
                weaponGraphic.sprite = null;
                // gak ngapa ngapain
            }
            else if (weapon.weaponType == WeaponType.Basoka)
            {
                // some logic
                weaponGraphic.sprite = weaponSprites[0];
                AimWeapon();
                FireWeapon(weapon);
            }
        }
    }

    private void AimWeapon()
    {
        Vector3 aimPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        weaponControl.LookAt(aimPos);
    }

    private void FireWeapon(WeaponInventory weapon)
    {
        if (weapon.weaponType == WeaponType.Basoka)
        {

        }
    }
}
