using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarHighlight : MonoBehaviour
{
    Player player;

    public Transform hotbarParent;
    InventorySlotUI[] slots;
    int currentHighlight = 0;
    
    void Start()
    {
        player = Player.instance;
        slots = hotbarParent.GetComponentsInChildren<InventorySlotUI>();

        player.onHotbarScrollCallback += ChangeHotbarSlot;
    }

    //Select Specific Slot
    //public void SelectItemSlot(){} 

    private void ChangeHotbarSlot()
    {
        HighlightEnabled(false);

        if (Input.mouseScrollDelta.y > 0)
        {
            if (currentHighlight != 0)
            {
                currentHighlight--;
            }
            else
            {
                currentHighlight = slots.Length - 1;
            }
            
            HighlightEnabled(true);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            currentHighlight++;
            if (currentHighlight > slots.Length - 1)
            {
                currentHighlight = 0;
            }

            HighlightEnabled(true);
        }
    }

    private void HighlightEnabled(bool value)
    {
        for (int i = 0; i < slots[currentHighlight].GetComponent<Transform>().childCount; i++)
        {
            if (slots[currentHighlight].GetComponent<Transform>().GetChild(i).name == "Highlight")
            {
                slots[currentHighlight].GetComponent<Transform>().GetChild(i).GetComponent<Image>().enabled = value;
            }
        }
    }

    public SO_Item GetCurrentlyEquippedItem()
    {
        if(slots[currentHighlight].GetComponent<InventorySlotUI>().GetItem() != null)
        {
            return slots[currentHighlight].GetComponent<InventorySlotUI>().GetItem();
        }
        return null;
    }

    private void OnDisable()
    {
        player.onHotbarScrollCallback -= ChangeHotbarSlot;
    }
}
