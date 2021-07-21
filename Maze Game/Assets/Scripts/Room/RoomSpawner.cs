using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class RoomSpawner : MonoBehaviour
{
    public RoomType roomType;
    public Limit limit;

    public int type;
    int rand = -1;

    void Start() {
        //limit = GameObject.FindGameObjectWithTag("rooms").GetComponent<Limit>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //Destroy(gameObject, 4f);
        //roomtype = GameObject.FindGameObjectWithTag("rooms").GetComponent<RoomType>();
        //Invoke("Spawn", 0.15f);

        Destroy(gameObject);
        Spawn();
    }

    void Spawn(){
        switch (type)
        {
            case 1: //Spawn kotak yang punya pintu bawah
                if (limit.isLimit_up == false)
                {
                    while (rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6)
                    {
                        rand = Random.Range(0, roomType.downroom.Length);
                    }
                }
                else if (limit.isLimit_up == true)
                {
                    while (rand < 0 || rand == 1 || rand == 4 || rand == 5)
                    {
                        rand = Random.Range(0, roomType.downroom.Length);
                    }
                }
                Instantiate(roomType.downroom[rand], transform.position, Quaternion.identity);
                break;
            case 2: //Spawn kotak yang punya pintu atas
                if (limit.isLimit_down == false)
                {
                    while (rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6)
                    {
                        rand = Random.Range(0, roomType.uproom.Length);
                    }
                }
                else if (limit.isLimit_down == true)
                {
                    while (rand < 0 || rand == 1 || rand == 4 || rand == 5)
                    {
                        rand = Random.Range(0, roomType.uproom.Length);
                    }
                }
                Instantiate(roomType.uproom[rand], transform.position, Quaternion.identity);
                break;
            case 3: //Spawn kotak yang punya pintu kanan
                if (limit.isLimit_left == false)
                {
                    while (rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6)
                    {
                        rand = Random.Range(0, roomType.rightroom.Length);
                    }
                }
                else if (limit.isLimit_left == true)
                {
                    while (rand < 0 || rand == 1 || rand == 4 || rand == 5)
                    {
                        rand = Random.Range(0, roomType.rightroom.Length);
                    }
                }
                Instantiate(roomType.rightroom[rand], transform.position, Quaternion.identity);
                break;
            case 4: //Spawn kotak yang punya pintu kiri
                if (limit.isLimit_right == false)
                {
                    while (rand < 0 || rand == 0 || rand == 2 || rand == 3 || rand == 6)
                    {
                        rand = Random.Range(0, roomType.leftroom.Length);
                    }
                }
                else if (limit.isLimit_right == true)
                {
                    while (rand < 0 || rand == 1 || rand == 4 || rand == 5)
                    {
                        rand = Random.Range(0, roomType.leftroom.Length);
                    }
                }
                Instantiate(roomType.leftroom[rand], transform.position, Quaternion.identity);
                break;
        }
    }

    /*

    void OnTriggerEnter2D(Collider2D col) {
        limit = GameObject.FindGameObjectWithTag("rooms").GetComponent<Limit>();
        roomType = GameObject.FindGameObjectWithTag("rooms").GetComponent<RoomType>();
        if(col.tag == "isRoom"){
            if(col.GetComponent<RoomSpawner>().spawned == false && spawned == false){
                Instantiate(roomType.closedroom, transform.position, Quaternion.identity);
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

    */
}
