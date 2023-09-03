using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateInventoryUI : MonoBehaviour
{
    [SerializeField] Transform hotbarParent;
    [SerializeField] Transform inventoryParent;

    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] GameObject hotbarSlotPrefab;

    [SerializeField] int hotbarSize = 6;

    Player player;
    SO_Inventory playerInventory;
    void Start()
    {
        player = Player.instance;
        playerInventory = player.inventory;

        player.onInventoryCreatedCallback += GenerateUI;
    }

    void GenerateUI()
    {
        Debug.Log("Generating inventory");
        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject _objI = Instantiate(inventorySlotPrefab, inventoryParent);
            _objI.GetComponent<InventorySlotUI>().SlotPosition = i;
        }

        for (int j = 0; j < hotbarSize; j++)
        {
            GameObject _objH = Instantiate(hotbarSlotPrefab, hotbarParent);
            _objH.GetComponent<InventorySlotUI>().SlotPosition = j;
        }

        player.onItemChangedCallback?.Invoke();
    }
}
