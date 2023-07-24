using System;
using System.Collections;
using UnityEngine;

public class CraftingGridGenerator : MonoBehaviour
{
    [SerializeField] Transform craftgridParent;

    [SerializeField] GameObject craftgridPrefab;

    Player player;
    SO_Inventory playerInventory;
    void Start()
    {
        player = Player.instance;
        playerInventory = player.inventory;

        player.onInventoryCreatedCallback += CreateCraftGridUI;
    }

    public void CreateCraftGridUI()
    {
        SO_Blueprint blueprint = Resources.Load<SO_Blueprint>("Blueprints/TestBlueprint");

        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject _objI = Instantiate(craftgridPrefab, craftgridParent);
            _objI.GetComponent<InformationCraftGridUI>().SetItemBlueprint(blueprint);
        }
    }
}
