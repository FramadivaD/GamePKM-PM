using UnityEngine;
using System.Collections;

public class WeaponOrb : MonoBehaviour
{
    [SerializeField] private PhotonView pv;

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
