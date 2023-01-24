using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventorySlotUI : MonoBehaviour, IDropHandler, IDragHandler, IInitializePotentialDragHandler
{
    //Externally setup
    public SO_Item item; //Item can be null
    public int SlotPosition; //Pos == InventoryPos

    int amount;

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemAmount;
    [SerializeField] RectTransform itemImageRect;
    [SerializeField] GameObject dragableItemPrefab;


    GameObject instantiatedObject;

    public void AddItem(SO_Item _newItem, int _amount)
    {
        if (_newItem != null)
        {
            item = _newItem;
            icon.sprite = item.Icon;
            icon.enabled = true;

            amount = _amount;

            itemAmount.SetText(_amount.ToString());
            itemAmount.enabled = true;
        }
        else
        {
            ClearSlot();
            DisableSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        amount = 0;

        icon.sprite = null;
        icon.enabled = false;

        itemAmount.SetText("0");
        itemAmount.enabled = false;
    }

    public void EnableSlot()
    {
        if (item != null)
        {
            icon.enabled = true;
            itemAmount.enabled = true;
        }
    }

    public void DisableSlot()
    {
        icon.enabled = false;
        itemAmount.enabled = false;
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }

    public SO_Item GetItem()
    {
        return item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item has been dropped on an inventory Slot");

        if ( eventData.pointerDrag != null && item == null)
        {
            Debug.Log("Item valid");
            DragDrop _itemDrop = eventData.pointerDrag.GetComponent<DragDrop>();

            if (_itemDrop != null)
            {
                _itemDrop.HasBeenDroppedOnSlot = true;

                _itemDrop.IsMySlot(this);
            }

            eventData.pointerDrag = null;
        }

    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //Debug.Log($"Potential Item to be dragged {item.ItemName}");
            instantiatedObject = Instantiate(dragableItemPrefab, Vector3.zero, Quaternion.identity, itemImageRect.transform);


            instantiatedObject.GetComponent<DragDrop>().ItemSlot = this;

            instantiatedObject.GetComponent<DragDrop>().HeldItem = item;
            instantiatedObject.GetComponent<DragDrop>().HeldItemAmount = amount;

            instantiatedObject.GetComponent<DragDrop>().canvas = GameObject.FindObjectOfType<Canvas>();

            //////////////////////////////////////////////////////////////////////////////////////////////
            
            instantiatedObject.GetComponent<RectTransform>().anchoredPosition =  itemImageRect.anchoredPosition;


            instantiatedObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;

            eventData.pointerDrag = instantiatedObject;

            DisableSlot();
        }
        else
        {

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

   
}
