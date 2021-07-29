using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extensione.Audio;

public class WeaponManager : MonoBehaviour
{
    Player player;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private SpriteRenderer weaponGraphic;
    [SerializeField] private List<Sprite> weaponSprites;
    [SerializeField] private List<GameObject> weaponProjectiles;

    [SerializeField] private Transform weaponControl;
    [SerializeField] private Transform weaponProjectileOrigin;

    [SerializeField] private AudioClip shootSFX;

    public void Initialize(Player player)
    {
        this.player = player;

        RefreshGraphic(null);
    }

    private WeaponInventory prevWeapon = null;
    public void ExecuteWeapon(WeaponInventory weapon)
    {
        if (weapon != null)
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount > 1) {
                    bool tapCondition = Input.GetTouch(1).phase == TouchPhase.Began;
                    bool holdCondition =
                        Input.GetTouch(1).phase == TouchPhase.Moved
                        || Input.GetTouch(1).phase == TouchPhase.Stationary;
                    bool releaseCondition = Input.GetTouch(1).phase == TouchPhase.Ended;

                    Controller(weapon, tapCondition, holdCondition, releaseCondition);
                } else
                {
                    Controller(weapon, false, false, true);
                }
            } else
            {
                bool tapCondition = Input.GetMouseButtonDown(0);
                bool holdCondition = Input.GetMouseButton(0);
                bool releaseCondition = Input.GetMouseButtonUp(0);

                Controller(weapon, tapCondition, holdCondition, releaseCondition);
            }
        } else
        {
            weaponTimer = basokaCooldown;
            RefreshGraphic(null);
        }
    }

    private void Controller(WeaponInventory weapon, bool tapCondition, bool holdCondition, bool releaseCondition)
    {
        if (holdCondition)
        {
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
        }
        else
        {
            weaponTimer = basokaCooldown;
            RefreshGraphic(null);
        }
    }

    private void AimWeapon()
    {
        weaponControl.rotation = GetAimRotation();
    }

    private Vector3 GetAimDirection()
    {
        Vector3 aimPos;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 1)
            {
                aimPos = playerCamera.ScreenToWorldPoint(Input.GetTouch(1).position);
            } else
            {
                aimPos = Vector3.zero;
            }
        } else
        {
            aimPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        }

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
                if (PhotonNetwork.connected)
                {
                    player.pv.RPC("SpawnProjectile", 
                        PhotonTargets.AllBuffered, 
                        weaponProjectileOrigin.position, GetAimRotation());
                }
                else
                {
                    SpawnProjectile(weaponProjectileOrigin.position, GetAimRotation());
                }

                weaponTimer += basokaCooldown;
            }
            weaponTimer -= Time.deltaTime;
        }
    }

    private void RefreshGraphic(WeaponInventory weapon)
    {
        if (prevWeapon != weapon)
        {
            int weaponType = weapon == null ? -1 : (int)weapon.weaponType;

            if (PhotonNetwork.connected)
            {
                player.pv.RPC("RefreshGraphicRPC", PhotonTargets.AllBuffered, weaponType);
            }
            else
            {
                RefreshGraphicRPC(weaponType);
            }

            prevWeapon = weapon;
        }
    }

    [PunRPC]
    private void RefreshGraphicRPC(int weaponType)
    {
        if (weaponType != -1)
        {
            if (weaponType == 0) weaponGraphic.sprite = null;
            else if (weaponType == 1) weaponGraphic.sprite = weaponSprites[0];

            Debug.Log("Refresh weapon dengan senjata asli");
        }
        else
        {
            weaponGraphic.sprite = null;

            Debug.Log("Refresh weapon dengan null");
        }
    }

    [PunRPC]
    private void SpawnProjectile(Vector3 pos, Quaternion rot)
    {
        GameObject ne = Instantiate(weaponProjectiles[0], pos, Quaternion.identity);

        WeaponProjectile bullet = ne.GetComponent<WeaponProjectile>();
        bullet.Initialize(rot);

        AudioManager.Instance.PlaySFXOnce(shootSFX);
    }
}
