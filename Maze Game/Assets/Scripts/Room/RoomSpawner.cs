using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    RoomType roomtype;
    public int type;
    public bool spawned = false;
    int rand = -1;
    Limit limit;

    void Start() {
        limit = GameObject.FindGameObjectWithTag("rooms").GetComponent<Limit>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Destroy(gameObject, 4f);
        roomtype = GameObject.FindGameObjectWithTag("rooms").GetComponent<RoomType>();
        //Invoke("Spawn", 0.15f);

        Spawn();
    }

    void Spawn(){
        if(!spawned){
            switch(type){
                case 1: //Spawn kotak yang punya pintu bawah
                    if(limit.isLimit_up == false){
                        while(rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6){
                            rand = Random.Range(0, roomtype.downroom.Length);
                        }
                    }else if(limit.isLimit_up == true){
                        while(rand < 0 || rand == 1 || rand == 4 || rand == 5){
                            rand = Random.Range(0, roomtype.downroom.Length);
                        }
                    }
                    Instantiate(roomtype.downroom[rand], transform.position, Quaternion.identity);
                    break;
                case 2: //Spawn kotak yang punya pintu atas
                    if(limit.isLimit_down == false){
                        while(rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6){
                            rand = Random.Range(0, roomtype.uproom.Length);
                        }
                    }else if(limit.isLimit_down == true){
                        while(rand < 0 || rand == 1 || rand == 4 || rand == 5){
                            rand = Random.Range(0, roomtype.uproom.Length);
                        }
                    }
                    Instantiate(roomtype.uproom[rand], transform.position, Quaternion.identity);
                    break;
                case 3: //Spawn kotak yang punya pintu kanan
                    if(limit.isLimit_left == false){
                        while(rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6){
                            rand = Random.Range(0, roomtype.rightroom.Length);
                        }
                    }else if(limit.isLimit_left == true){
                        while(rand < 0 || rand == 1 || rand == 4 || rand == 5){
                            rand = Random.Range(0, roomtype.rightroom.Length);
                        }
                    }
                    Instantiate(roomtype.rightroom[rand], transform.position, Quaternion.identity);
                    break;
                case 4: //Spawn kotak yang punya pintu kiri
                    if(limit.isLimit_right == false){
                        while(rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6){
                            rand = Random.Range(0, roomtype.leftroom.Length);
                        }
                    }
                    else if(limit.isLimit_right == true){
                        while(rand < 0 || rand == 1 || rand == 4 || rand == 5){
                            rand = Random.Range(0, roomtype.leftroom.Length);
                        }
                    }
                    Instantiate(roomtype.leftroom[rand], transform.position, Quaternion.identity);
                    break;
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        limit = GameObject.FindGameObjectWithTag("rooms").GetComponent<Limit>();
        roomtype = GameObject.FindGameObjectWithTag("rooms").GetComponent<RoomType>();
        if(col.tag == "isRoom"){
            if(col.GetComponent<RoomSpawner>().spawned == false && spawned == false){
                Instantiate(roomtype.closedroom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
        else if(col.tag == "limit_left"){
            limit.isLimit_left = true;
        }
        else if(col.tag == "limit_right"){
            limit.isLimit_right = true;
        }
        else if(col.tag == "limit_up"){
            limit.isLimit_up = true;
        }
        else if(col.tag == "limit_down"){
            limit.isLimit_down = true;
        }
    }
}
