using System;
using System.Collections;
using UnityEngine;

public class CraftingGridGenerator : MonoBehaviour
{
    [SerializeField] Transform craftgridParent;

    [SerializeField] GameObject craftgridPrefab;
    [SerializeField] GameObject craftMenu;
 
    Player player;
    SO_Inventory playerInventory;

    private CraftStation craftStation;

    private void Awake()
    {
        craftStation = GameObject.FindObjectOfType<CraftStation>();

    }

    void Start()
    {
        player = Player.instance;
        playerInventory = player.inventory;

        player.onInventoryCreatedCallback += CreateCraftGridUI;
        craftStation.onMenuTogggleCallback += MenuToggle;
    }

    private void MenuToggle()
    {
        craftMenu.SetActive(craftMenu.activeSelf?false:true);
    }

    public void CreateCraftGridUI()
    {
        SO_Blueprint blueprint = Resources.Load<SO_Blueprint>("Blueprints/TestBlueprint");

        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject _objI = Instantiate(craftgridPrefab, craftgridParent);
            _objI.GetComponent<CraftGridElement>().SetItemBlueprint(blueprint);
        }
    }
}
