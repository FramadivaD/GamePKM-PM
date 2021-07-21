using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRoom : MonoBehaviour
{
    RoomType target;
    float distanceX = 0, distanceY = 0;
    float rotateY = 0, rotateZ = 0;
    Vector2 TargetPos;
    
    public GameObject[] Gate;
    public GameObject[] obor;

    int idTeam = 0;

    private void Start() {
        target = GetComponent<RoomType>();
        Invoke("SpawnTarget", 2.6f);
    }

    void SpawnTarget()
    {
        List<GameObject> possibleTarget = new List<GameObject>();
        for (int i = (target.allRooms.Count - 1); i >= 0; i--)
        {
            if (target.allRooms[i].GetComponent<AddRoom>().tipe != 0)
            {
                possibleTarget.Add(target.allRooms[i]);
            }
        }

        target.TargetRoom = possibleTarget;

        for (int i = 0; i < Gate.Length && i < obor.Length; i++)
        {
            if (i < possibleTarget.Count)
            {
                spawnTarget(possibleTarget[i]);
            }
        }
    }

    void spawnTarget(GameObject roomtarget){
        if (roomtarget)
        {
            switch (roomtarget.GetComponent<AddRoom>().tipe)
            {
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
            }
            TargetPos = new Vector2(roomtarget.transform.position.x + distanceX, roomtarget.transform.position.y + distanceY);
            Instantiate(Gate[idTeam], TargetPos, Quaternion.Euler(0, rotateY, rotateZ));
            Instantiate(obor[idTeam], roomtarget.transform.position, Quaternion.identity);
            idTeam++;
        }
    }
}
