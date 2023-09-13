using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{

    public int[] InventoryItemAmount;
    public int[] InventoryItemSlot;
    public string[] InventoryItemID;


    public Data(Player player, GameManager manager)
    {
        InventoryItemAmount = new int[player.inventory.itemCap];
        InventoryItemSlot = new int[player.inventory.inventorySize];
        InventoryItemID = new string[player.inventory.inventorySize];

        int counter = 0;

        foreach(var item in player.inventory.inventoryItems)
        {
            InventoryItemAmount[counter] = item.amount;
            InventoryItemID[counter] = item.item.ItemId;
            InventoryItemSlot[counter] = item.slotNum;

            counter++;
        }
    }
}
