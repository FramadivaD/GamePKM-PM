using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    Player player;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private SpriteRenderer weaponGraphic;
    [SerializeField] private List<Sprite> weaponSprites;
    [SerializeField] private List<GameObject> weaponProjectiles;

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
                    // gak perlu ngapa ngapain
                }
                else if (weapon.weaponType == WeaponType.Basoka)
                {
                    AimWeapon();
                    FireWeapon(weapon);
                }

                RefreshGraphic(weapon);
            } else
            {
                weaponTimer = 0;
                RefreshGraphic(null);
            }
        } else
        {
            weaponTimer = 0;
            RefreshGraphic(null);
        }
    }

    private void AimWeapon()
    {
        weaponControl.rotation = GetAimRotation();
    }

    private Vector3 GetAimDirection()
    {
        Vector3 aimPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = aimPos - transform.position;
        return direction;
    }

    private Quaternion GetAimRotation()
    {
        Vector3 direction = GetAimDirection();

        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);

        return rotation;
    }

    private float weaponTimer = 0;
    private float basokaCooldown = 1;
    private void FireWeapon(WeaponInventory weapon)
    {
        if (weapon.weaponType == WeaponType.Basoka)
        {
            if (weaponTimer <= 0)
            {
                GameObject ne = Instantiate(weaponProjectiles[0], weaponControl.position, Quaternion.identity);

                WeaponProjectile bullet = ne.GetComponent<WeaponProjectile>();
                bullet.Initialize(GetAimDirection(), GetAimRotation());

                weaponTimer += basokaCooldown;
            }
            weaponTimer -= Time.deltaTime;
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
