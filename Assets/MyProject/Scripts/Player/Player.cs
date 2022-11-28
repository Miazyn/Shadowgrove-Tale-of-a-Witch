using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;
    void Awake()
    {
        instance = this;
    }

    public SO_Inventory playerInventory;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public delegate void OnHotbarScroll();
    public OnItemChanged onHotbarScrollCallback;

    public delegate void OnInventoryToggle();
    public OnItemChanged onInventoryToggleCallback;

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (onHotbarScrollCallback != null)
            {
                onHotbarScrollCallback.Invoke();
            }
        }

        if (Input.GetButtonDown("Inventory"))
        {
            if (onInventoryToggleCallback != null)
            {
                onInventoryToggleCallback.Invoke();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Item has been triggered");

        var item = collision.gameObject.GetComponent<ItemHolder>();
        if (item)
        {
            Debug.Log("Can Add Item");
            bool wasItemAdded = playerInventory.AddItem(item.item, 1);
            if (wasItemAdded)
            {
                if (onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }
                else
                {
                    Debug.Log("No methods subscribed");
                }
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        //B4 clear, save inventory
        playerInventory.inventoryItems.Clear();
    }
}
