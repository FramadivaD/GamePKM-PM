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

        RefreshGraphic(null);
    }

    public void ExecuteWeapon(WeaponInventory weapon)
    {
        if (weapon != null)
        {
            if (Input.GetMouseButton(0)) {
                if (weapon.weaponType == WeaponType.None)
                {
                    // gak ngapa ngapain
                }
                else if (weapon.weaponType == WeaponType.Basoka)
                {
                    // some logic
                    AimWeapon();
                    FireWeapon(weapon);
                }

                RefreshGraphic(weapon);
            } else
            {
                RefreshGraphic(null);
            }
        } else
        {
            RefreshGraphic(null);
        }
    }

    private void AimWeapon()
    {
        Vector3 aimPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = aimPos - transform.position;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);

        weaponControl.rotation = rotation;
    }

    private void FireWeapon(WeaponInventory weapon)
    {
        if (weapon.weaponType == WeaponType.Basoka)
        {

        }
    }

    private void RefreshGraphic(WeaponInventory weapon)
    {
        if (weapon != null)
        {
            if (weapon.weaponType == WeaponType.None) weaponGraphic.sprite = null;
            else if (weapon.weaponType == WeaponType.Basoka) weaponGraphic.sprite = weaponSprites[0];
        } else
        {
            weaponGraphic.sprite = null;
        }
    }
}
