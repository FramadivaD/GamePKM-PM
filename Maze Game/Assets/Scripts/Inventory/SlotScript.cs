using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventdata){
        Debug.Log("DroppedSlot");
        if(eventdata.pointerDrag != null){
            eventdata.pointerDrag.GetComponent<Transform>().position = transform.position;
        }
    }
}
