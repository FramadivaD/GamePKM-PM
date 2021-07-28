using UnityEngine;
using System.Collections;

public class EnemyRadar : MonoBehaviour
{
    [SerializeField]private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                if (other.CompareTag("Player"))
                {
                    enemy.playerTarget = other.gameObject;
                }
            }
        } else
        {
            if (other.CompareTag("Player"))
            {
                enemy.playerTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                if (other.CompareTag("Player"))
                {
                    if (other.gameObject == enemy.playerTarget) {
                        enemy.playerTarget = null;
                    }
                }
            }
        } else
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject == enemy.playerTarget)
                {
                    enemy.playerTarget = null;
                }
            }
        }
    }
}
