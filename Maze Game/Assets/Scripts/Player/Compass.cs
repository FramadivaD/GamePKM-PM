using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    int teamId = 0;
    RoomType roomtype;
    Vector3 dir;
    GameObject dest;
    GameObject Player;
    void Start()
    {
        roomtype = GameObject.FindGameObjectWithTag("rooms").GetComponent<RoomType>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Invoke("FindLocation", 2.6f);
    }

    void FindLocation(){
        dest = roomtype.TargetRoom[teamId];
        dir = dest.transform.position;
        Vector3 angleTarget = dir - Player.transform.position;
        float angle = Mathf.Atan2(angleTarget.y, angleTarget.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
