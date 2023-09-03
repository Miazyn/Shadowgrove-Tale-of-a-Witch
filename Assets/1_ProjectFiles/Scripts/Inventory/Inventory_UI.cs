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
    [SerializeField] GameObject hotbarBackground;

    void Start()
    {
        player = Player.instance;
        player.onItemChangedCallback += UpdateUI;
        inventory = player.inventory;

        //inventoryUI.SetActive(false);
        slots = itemsParent.GetComponentsInChildren<InventorySlotUI>();

        hotbarSlots = hotbarParent.GetComponentsInChildren<InventorySlotUI>();
        
    }
    
    private void OnDisable()
    {
        player.onItemChangedCallback -= UpdateUI;
    }

    void IndividualUIUpdate(int _slotPos)
    {
        
    }

    void UpdatingSlots()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlotUI>();
        hotbarSlots = hotbarParent.GetComponentsInChildren<InventorySlotUI>();

        ///INVENTORY SLOTS

        for (int i = 0; i < inventory.inventorySize; i ++)
        {
            for(int j = 0; j < slots.Length; j++)
            {
                if(inventory.inventoryItems[i].slotNum == slots[j].SlotPosition)
                {
                    InventorySlot _slotInfo = inventory.inventoryItems[i];
                    slots[j].AddItem(_slotInfo.item, _slotInfo.amount);
                    break;
                }
            }
        }

        ///HOTBAR SLOTS

        for (int i = 0; i < inventory.inventorySize; i++)
        {
            for (int j = 0; j < hotbarSlots.Length; j++)
            {
                if (inventory.inventoryItems[i].slotNum == hotbarSlots[j].SlotPosition)
                {
                    InventorySlot _slotInfo = inventory.inventoryItems[i];
                    hotbarSlots[j].AddItem(_slotInfo.item, _slotInfo.amount);
                    break;
                }
            }
        }
    }

    void UpdateUI()
    {
        UpdatingSlots();
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

    public void InventoryToggle()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        hotbar.SetActive(!inventoryUI.activeSelf);
        hotbarBackground.SetActive(!inventoryUI.activeSelf);
        itemNameAnzeige.SetActive(!inventoryUI.activeSelf);
    }
   
}
