using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IDropHandler, IDragHandler, IInitializePotentialDragHandler
{
    SO_Item item;
    public Image icon;
    public TextMeshProUGUI itemAmount;
    int amount;

    [SerializeField] RectTransform itemImageRect;
    [SerializeField] GameObject DragableItemPrefab;

    public void AddItem(SO_Item _newItem, int _amount)
    {
        item = _newItem;
        icon.sprite = item.Icon;
        icon.enabled = true;

        amount = _amount;

        itemAmount.SetText(_amount.ToString());
        itemAmount.enabled = true;
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
        if( eventData.pointerDrag != null && item == null)
        {
            Debug.Log("Item has been dropped on an inventory Slot");
            DragDrop _itemDrop = eventData.pointerDrag.GetComponent<DragDrop>();
            AddItem(_itemDrop.HeldItem, _itemDrop.HeldItemAmount);
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log($"Potential Item to be dragged {item.ItemName}");
            GameObject instantiatedObject = Instantiate(DragableItemPrefab, transform);

            instantiatedObject.GetComponent<DragDrop>().HeldItem = item;
            instantiatedObject.GetComponent<DragDrop>().HeldItemAmount = amount;

            instantiatedObject.GetComponent<DragDrop>().canvas = GameObject.FindObjectOfType<Canvas>();

            //////////////////////////////////////////////////////////////////////////////////////////////

            Vector3 mousePos = new Vector3(Input.mousePosition.x - 2, Input.mousePosition.y - 2, Input.mousePosition.z);
            instantiatedObject.transform.position = mousePos;

            eventData.pointerDrag = instantiatedObject;
        }
        else
        {
            Debug.Log("No item to be dragged");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

   
}
