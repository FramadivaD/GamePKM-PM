using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemScript : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Canvas canvas;
    RectTransform rect;
    Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        startPos = rect.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void itemDrag(PointerEventData eventdata){
        rect.anchoredPosition += eventdata.delta / canvas.scaleFactor;
    }

    public void itemDrop(PointerEventData eventdata){
        rect.position = new Vector2(startPos.x, startPos.y);
    }*/

    public void OnBeginDrag(PointerEventData eventdata){
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventdata){
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventdata){
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
