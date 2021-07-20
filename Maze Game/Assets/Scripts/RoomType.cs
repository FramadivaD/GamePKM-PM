using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public GameObject[] uproom;
    public GameObject[] downroom;
    public GameObject[] leftroom;
    public GameObject[] rightroom;

    public GameObject closedroom;
    Renderer roomrender;
    public GameObject TargetRoom;
    public List<GameObject> allRooms;

    private void Start() {
        Invoke("FindTargetRoom", 2.5f);
    }

    void FindTargetRoom(){
        for(int i = (allRooms.Count-1); i >= 0; i--){
            if(allRooms[i].GetComponent<AddRoom>().tipe != 0){
                TargetRoom = allRooms[i];
                break;
            }
        }
    }
}
