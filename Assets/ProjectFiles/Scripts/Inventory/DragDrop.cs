using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    public SO_Item HeldItem;
    public int HeldItemAmount;
    public Canvas canvas;


    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    public bool HasBeenDroppedOnSlot = false;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Destroy(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
}
