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

    [SerializeField] GameObject itemNameAnzeige; 

    void Start()
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

    void IndividualUIUpdate(int _slotPos)
    {

    }

    private void UpdateUI()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlotUI>();

        hotbarSlots = hotbarParent.GetComponentsInChildren<InventorySlotUI>();


        for (int i = 0; i < slots.Length; i++)
        {
            
            if(i < inventory.inventoryItems.Count && inventory.inventoryItems[i].item != null)
            {
                Debug.Log("Added images to Slot");
                slots[i].AddItem(inventory.inventoryItems[i].item, inventory.inventoryItems[i].amount);
            }
            else
            {
                Debug.Log("Cleared Slot");
                slots[i].ClearSlot();
            }

            Debug.Log($"Item inside is {inventory.inventoryItems[i].item}");
        }

        for(int i = 0; i < hotbarSlots.Length; i++)
        {
            if(i < inventory.inventoryItems.Count && inventory.inventoryItems[i].item != null)
            {
                hotbarSlots[i].AddItem(inventory.inventoryItems[i].item, inventory.inventoryItems[i].amount);
            }
            else
            {
                hotbarSlots[i].ClearSlot();
            }
        }
    }

    void UpdateSlots(InventorySlotUI[] _slots)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < inventory.inventoryItems.Count && inventory.inventoryItems[i].item != null)
            {
                _slots[i].AddItem(inventory.inventoryItems[i].item, inventory.inventoryItems[i].amount);
            }
            else
            {
                _slots[i].ClearSlot();
            }
        }
    }

    private void InventoryToggle()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        hotbar.SetActive(!inventoryUI.activeSelf);
        itemNameAnzeige.SetActive(!inventoryUI.activeSelf);
    }
    private void OnDisable()
    {
        player.onItemChangedCallback -= UpdateUI;
        player.onInventoryToggleCallback -= InventoryToggle;
    }
}
