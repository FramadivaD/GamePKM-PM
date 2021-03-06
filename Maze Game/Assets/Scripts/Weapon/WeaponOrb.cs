using UnityEngine;
using System.Collections;

using Extensione.Audio;

public class WeaponOrb : MonoBehaviour
{
    [SerializeField] private PhotonView pv;

    [SerializeField] private WeaponInventory weapon;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSFX;

    public WeaponInventory Weapon { get { return weapon; } }

    public void Initialize(int weaponType)
    {
        if (PhotonNetwork.connected)
        {
            pv.RPC("InitializeRPC", PhotonTargets.AllBuffered, weaponType);
        } else
        {
            InitializeRPC(weaponType);
        }
    }

    [PunRPC]
    private void InitializeRPC(int weaponType)
    {
        weapon = new WeaponInventory() { weaponType = (WeaponType) weaponType };
    }

    public WeaponInventory TakeWeapon(Player player)
    {
        if (!player.inventoryManager.IsFull)
        {
            player.inventoryManager.AddItem(Weapon);

            AudioManager.Instance.PlaySFXOnce(pickupSFX);

            if (PhotonNetwork.connected)
            {
                pv.RPC("DestroyOrb", PhotonTargets.MasterClient);
            }
            else
            {
                Destroy(gameObject);
            }
            return weapon;
        }
        return null;
    }

    [PunRPC]
    private void DestroyOrb()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
