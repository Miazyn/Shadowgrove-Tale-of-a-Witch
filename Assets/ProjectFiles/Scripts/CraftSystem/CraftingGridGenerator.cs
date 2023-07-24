using System;
using System.Collections;
using UnityEngine;

public class CraftingGridGenerator : MonoBehaviour
{

    [SerializeField] Transform hotbarParent;
    [SerializeField] Transform craftgridParent;

    [SerializeField] GameObject craftgridPrefab;

    Player player;
    SO_Inventory playerInventory;
    void Start()
    {
        player = Player.instance;
        playerInventory = player.inventory;

        player.onInventoryCreatedCallback += Method;
    }

    public void Method()
    {
        
    }
}
