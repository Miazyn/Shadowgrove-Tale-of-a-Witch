using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "SO/Inventory")]
public class SO_Inventory : ScriptableObject
{
    public string inventoryName;
    public int inventorySize;

    public List<InventorySlot> inventoryItems = new List<InventorySlot>();
    public bool AddItem(SO_Item _item, int _amount)
    {

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].item == _item)
            {
                inventoryItems[i].AddAmount(_amount);
                Player.instance.onItemChangedCallback?.Invoke();
                return true;
            }
        }
        if (inventoryItems.Count >= inventorySize)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        Debug.Log("Enough room.");
        inventoryItems.Add(new InventorySlot(_item, _amount));
        Player.instance.onItemChangedCallback?.Invoke();
        return true;

    }

    public void RemoveItem(SO_Item _item)
    {
        List<InventorySlot> _tempList = new List<InventorySlot>();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].item != _item)
            {
                _tempList.Add(inventoryItems[i]);
            }
            else
            {
                if(inventoryItems[i].amount - 1 > 0)
                {
                    _tempList.Add(new InventorySlot(inventoryItems[i].item, inventoryItems[i].amount - 1));
                }
            }
        }

        inventoryItems = new List<InventorySlot>();
        inventoryItems = _tempList;
        Player.instance.onItemChangedCallback?.Invoke();

    }
}

[System.Serializable]
public class InventorySlot
{
    public SO_Item item;
    public int amount;
    public InventorySlot (SO_Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
