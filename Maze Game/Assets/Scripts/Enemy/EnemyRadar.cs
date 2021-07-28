using UnityEngine;
using System.Collections;

public class EnemyRadar : MonoBehaviour
{
    [SerializeField]private Enemy enemy;

    private void Update()
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.player.IsMasterClient)
            {
                FindNearestPlayer();
            }
        } else
        {
            FindNearestPlayer();
        }
    }

    private void FindNearestPlayer()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 7)
            {
                enemy.playerTarget = player.gameObject;
                return;
            }
        }

        enemy.playerTarget = null;
    }
}
