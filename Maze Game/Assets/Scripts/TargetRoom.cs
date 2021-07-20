using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRoom : MonoBehaviour
{
    RoomType target;
    GameObject roomtarget;
    float distanceX = 0, distanceY = 0;
    float rotateY = 0, rotateZ = 0;
    Vector2 TargetPos;
    
    public GameObject Gate;
    public GameObject obor;

    private void Start() {
        target = GetComponent<RoomType>();
        Invoke("spawnTarget", 2.6f);
    }

    void spawnTarget(){
        roomtarget = target.TargetRoom;
        switch(roomtarget.GetComponent<AddRoom>().tipe){
            case 1:
                distanceY = 5.4f;
                rotateZ = 90f;
                break;
            case 2:
                distanceY = -5.4f;
                rotateZ = -90f;
                break;
            case 3:
                distanceX = 10.35f;
                rotateY = 0f;
                break;
            case 4:
                distanceX = -10.35f;
                rotateY = 180f;
                break;
        }TargetPos = new Vector2(roomtarget.transform.position.x + distanceX, roomtarget.transform.position.y + distanceY);
        Instantiate(Gate, TargetPos, Quaternion.Euler(0, rotateY, rotateZ));
        Instantiate(obor, roomtarget.transform.position, Quaternion.identity);
    }
}
