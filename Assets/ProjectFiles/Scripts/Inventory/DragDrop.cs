using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] float delayedCheck = 2f;

    public InventorySlotUI ItemSlot;

    public SO_Item HeldItem;
    public int HeldItemAmount;
    public Canvas canvas;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    public bool HasBeenDroppedOnSlot = false;
    bool IsDragged = false;

    Player player;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        player = Player.instance;
        StartCoroutine(CheckIfDragged());
    }

    IEnumerator CheckIfDragged()
    {
        yield return new WaitForSeconds(delayedCheck);
        if (!IsDragged)
        {
            Destroy(gameObject);
        }
    }

    public bool IsMySlot(InventorySlotUI _slot)
    {
        if(_slot != ItemSlot)
        {
            int prevIndex = ItemSlot.SlotPosition;
            int curSlot = _slot.SlotPosition;

            for(int i = 0; i < player.inventory.inventorySize; i++)
            {

                //Check through inventory. Nullify old ref, fill new ref
                if(player.inventory.inventoryItems[i].slotNum == prevIndex)
                {
                    //NULLIFY
                    player.inventory.inventoryItems[i].item = null;
                    player.inventory.inventoryItems[i].amount = 0;
                }

                if(player.inventory.inventoryItems[i].slotNum == curSlot)
                {
                    //FILL INFO IN
                    player.inventory.inventoryItems[i].item = HeldItem;
                    player.inventory.inventoryItems[i].amount = HeldItemAmount;
                }

            }

            player.onItemChangedCallback?.Invoke();

            Destroy(gameObject);

            return false;
        }

        //enable textures on slot again
        ItemSlot.EnableSlot();

        player.onItemChangedCallback?.Invoke();

        Destroy(gameObject);
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        IsDragged = true;
        ///!!!!!BAD CODE!!!!!!!!!!
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

    }

    public void OnDrop(PointerEventData eventData)
    {
    }
}
