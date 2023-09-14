using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class ShopTile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float timeb4WindowOpens = 0.5f;

    public SO_Item Item;
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Price;
    [SerializeField] TextMeshProUGUI PlayerStock;

    Player player;
    SO_Inventory PlayerInventory;

    public GameObject cursor;

    private void Start()
    {
        player = Player.instance;

        PlayerInventory = player.inventory;
    }

    public void BuyItem()
    {
        if (player.CanBuy(Item.BuyPrice))
        {
            player.SetMoneyAmount(-Item.BuyPrice);

            player.inventory.AddItem(Item, 1);
            SetUpTile();
        }
    }

    public void SetUpTile()
    {
        player = Player.instance;
        PlayerInventory = player.inventory;

        ItemImage.sprite = Item.Icon;
        Name.SetText(Item.ItemName);
        Price.SetText(Item.BuyPrice.ToString());
        //int playerStockOfItem = 0;

        //if (player.inventory.inventoryItems.Count > 0)
        //{
        //    playerStockOfItem = PlayerInventory.GetItemCount(Item);

        //    PlayerStock.SetText("Owned: " + playerStockOfItem.ToString());
        //}
        //else
        //{
        //    Debug.LogWarning("NO Items in player");
        //    PlayerStock.SetText("Owned: " + 0);
        //}

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(TimerB4ShowDesc());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        DescriptionBoxOnHover.OnMouseLoseFocus();
    }

    void ShowMessage()
    {
        // DescriptionBoxOnHover.OnMouseOver(Item, Mouse.current.position.ReadValue());
    }

    IEnumerator TimerB4ShowDesc()
    {
        yield return new WaitForSeconds(timeb4WindowOpens);
        ShowMessage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }
}
