using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float cs;
    public GameObject Player;
    public Vector3 originPos = new Vector3(0,0,0);
    Vector3 charapos, camDistance;

    void Update()
    {
        CameraPlayer();
    }

    void CameraPlayer(){
        charapos = new Vector3(Player.transform.position.x, Player.transform.position.y, -12f);
        if(camDistance.x <= -8.885f)
            originPos -= Vector3.right * 17.77f;
        else if(camDistance.x >= 8.885f)
            originPos += Vector3.right * 17.77f;
        if(camDistance.y <= -5f)
            originPos -= Vector3.up * 10f;
        else if(camDistance.y >= 5f)
            originPos += Vector3.up * 10f;
        camDistance = charapos - originPos;
        Vector3 camPos = charapos;
        if(camDistance.x <= -2.66f)
            camPos = new Vector3(-2.66f + originPos.x, camPos.y, -12f);
        else if(camDistance.x >= 2.68f)
            camPos = new Vector3(2.68f + originPos.x, camPos.y, -12f);
        if(camDistance.y <= -1.5f)
            camPos = new Vector3(camPos.x, -1.5f + originPos.y, -12f);
        else if(camDistance.y >= 1.5f)
            camPos = new Vector3(camPos.x, 1.5f + originPos.y, -12f);
        transform.position = camPos;
    }
}
