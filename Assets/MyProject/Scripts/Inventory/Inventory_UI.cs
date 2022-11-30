using System;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    SO_Inventory inventory;
    Player player;

    public Transform itemsParent;
    InventorySlotUI[] slots;

    public GameObject inventoryUI;

    InventorySlotUI[] hotbarSlots;
    public GameObject hotbar;
    public Transform hotbarParent;

    private void Start()
    {
        player = Player.instance;
        player.onItemChangedCallback += UpdateUI;
        player.onInventoryToggleCallback += InventoryToggle;
        inventory = player.inventory;

        inventoryUI.SetActive(false);
        slots = itemsParent.GetComponentsInChildren<InventorySlotUI>();

        hotbar.SetActive(true);
        hotbarSlots = hotbarParent.GetComponentsInChildren<InventorySlotUI>();
        
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

        for(int i = 0; i < hotbarSlots.Length; i++)
        {
            if(i < inventory.inventoryItems.Count)
            {
                hotbarSlots[i].AddItem(inventory.inventoryItems[i].item, inventory.inventoryItems[i].amount);
            }
            else
            {
                hotbarSlots[i].ClearSlot();
            }
        }
    }
    private void InventoryToggle()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        hotbar.SetActive(!inventoryUI.activeSelf);
    }
    private void OnDisable()
    {
        player.onItemChangedCallback -= UpdateUI;
        player.onInventoryToggleCallback -= InventoryToggle;
    }
}
