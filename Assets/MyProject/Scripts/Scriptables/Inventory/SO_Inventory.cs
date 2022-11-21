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
        return true;

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
