using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopTilePrefab;

    SO_Item[] allItems;

    public Transform parentTransform;

    Player player;
    [Space]
    [SerializeField] private GameObject cursor;

    void Awake()
    {
        allItems = Resources.LoadAll<SO_Item>("Items");
    }

    void Start()
    {
        player = Player.instance;

        //player.onMoneyChangedCallback += UpdateMoneyUI;
        for(int i =0; i < allItems.Length; i++)
        {
            GameObject _shopTile = Instantiate(ShopTilePrefab, parentTransform);

            _shopTile.GetComponent<RectTransform>().anchoredPosition = parentTransform.GetComponent<RectTransform>().anchoredPosition;
            _shopTile.GetComponent<ShopTile>().Item = allItems[i];

            _shopTile.GetComponent<ShopTile>().cursor = cursor;

            _shopTile.GetComponent<ShopTile>().SetUpTile();
        }
    }

}
