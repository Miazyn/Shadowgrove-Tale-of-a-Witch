using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "SO/Inventory")]
public class SO_Inventory : ScriptableObject
{
    public string inventoryName;
    public int inventorySize;

    public List<InventorySlot> inventoryItems = new List<InventorySlot>();
    public bool AddItem(SO_Item _item, int _amount, int _slotPos)
    {
        if(_slotPos >= inventoryItems.Count)
        {
            Debug.Log("Slot out of range");

            return false;
        }

        InventorySlot _itemSlot = inventoryItems[_slotPos];

        //COMPARE ITEM TO BE ADDED WITH ITEM IN INVENTORY
        if (_itemSlot.item != _item && _itemSlot.item != null)
        {
            Debug.Log($"Items are not equal. {_itemSlot.item} and {_item}");

            return false;
        }

        if(_itemSlot.item == null)
        {
            _itemSlot.item = _item;
            _itemSlot.AddAmount(_amount);
            Player.instance.onItemChangedCallback?.Invoke();

            return true;
        }


        _itemSlot.AddAmount(_amount);
        Player.instance.onItemChangedCallback?.Invoke();

        return true;

    }
    public void RemoveItem(SO_Item _item, int _slotPos)
    {
        InventorySlot _itemSlot = inventoryItems[_slotPos];

        if(_itemSlot.amount - 1 == 0)
        {
            _itemSlot.item = null;
            _itemSlot.amount--;
        }

        if(_itemSlot.amount - 1 > 0)
        {
            _itemSlot.amount--;
        }

        Player.instance.onItemChangedCallback?.Invoke();
    }
    public void RemoveItem(SO_Item _item)
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].item == _item)
            {
                if (inventoryItems[i].amount - 1 == 0)
                {
                    inventoryItems[i].amount--;
                    inventoryItems[i].item = null;
                }
                else
                {
                    inventoryItems[i].amount--;
                }
            }
        }

        Player.instance.onItemChangedCallback?.Invoke();
    }

    public bool AddItem(SO_Item _item, int _amount)
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].item == _item)
            {
                AddItem(_item, _amount, i);
                return true;
            }
        }

        for(int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].item == null)
            {
                AddItem(_item, _amount, i);
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }
}

[System.Serializable]
public class InventorySlot
{
    public SO_Item item;
    public int amount;
    public int slotNum;
    public InventorySlot (SO_Item _item, int _amount, int _slotNum)
    {
        item = _item;
        amount = _amount;
        slotNum = _slotNum;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
