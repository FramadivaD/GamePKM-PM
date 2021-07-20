using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag(){
        Vector2 mousedrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousedrag.x, mousedrag.y);
    }
}
