using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player"){
            Destroy(other.gameObject);
        }
    }
    
    private void Start() {
        Destroy(gameObject, 4f);
    }
}
