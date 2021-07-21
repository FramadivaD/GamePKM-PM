using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    RoomType addroom;
    public int tipe;
    private void Start() {
        addroom = GameObject.FindGameObjectWithTag("rooms").GetComponent<RoomType>();
        addroom.allRooms.Add(this.gameObject);
    }
}