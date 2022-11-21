using System;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    SO_Inventory inventory;
    Player player;

    public Transform itemsParent;
    InventorySlotUI[] slots;

    public GameObject inventoryUI;
    private void Start()
    {
        player = Player.instance;
        player.onItemChangedCallback += UpdateUI;
        inventory = player.playerInventory;

        inventoryUI.SetActive(false);
        slots = itemsParent.GetComponentsInChildren<InventorySlotUI>();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.inventoryItems.Count)
            {
                slots[i].AddItem(inventory.inventoryItems[i].item, inventory.inventoryItems[i].amount);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }


}
