using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    SO_Item item;
    public Image icon;
    public TextMeshProUGUI itemAmount;
    public void AddItem(SO_Item newItem, int amount)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        itemAmount.SetText(amount.ToString());
        itemAmount.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

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
}
