using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    InventoryManager inventory;
    
    void Start()
    {
        inventory = GameObject.Find("Game Manager").GetComponent<InventoryManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("item")){
            for(int i = 0; i < inventory.inventorySlots.Length; i++){
                if(!inventory.isFull[i]){
                    Instantiate(inventory.itemButton, inventory.inventorySlots[i].transform, false);
                    inventory.isFull[i] = true;
                    Destroy(other.gameObject);
                    break;
                }
            }
        }
    }
}
