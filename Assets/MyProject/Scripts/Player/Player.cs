using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    string InventoryButton = "Inventory";
    string InteractionButton = "Interact";
    public static Player instance;
    [SerializeField] HotbarHighlight currentItem;
    Interactor interactor;

    void Awake()
    {
        instance = this;
        if(currentItem == null)
        {
            Debug.LogWarning("No Hotbar is defined.");
        }
        //Grab cuz its on player
        interactor = GetComponent<Interactor>();
    }

    public SO_Inventory inventory;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public delegate void OnHotbarScroll();
    public OnItemChanged onHotbarScrollCallback;

    public delegate void OnInventoryToggle();
    public OnItemChanged onInventoryToggleCallback;

    private void Update()
    {
        if (Input.GetButtonDown(InteractionButton))
        {
            if (interactor.GetOverlaps().Item1)
            {
                interactor.GetOverlaps().Item2.Interact(interactor);
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            if (onHotbarScrollCallback != null)
            {
                onHotbarScrollCallback.Invoke();
            }
        }

        if (Input.GetButtonDown(InventoryButton))
        {
            if (onInventoryToggleCallback != null)
            {
                onInventoryToggleCallback.Invoke();
            }
        }
    }

    //LOGIC INTO INTERACTIONS
    private void OnCollisionEnter(Collision collision)
    {
        var item = collision.gameObject.GetComponent<ItemHolder>();
        if (item)
        {
            Debug.Log("Can Add Item");
            bool wasItemAdded = inventory.AddItem(item.item, 1);
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

    public SO_Item GetCurrentItem()
    {
        return currentItem.GetCurrentlyEquippedItem();
    }

    private void OnApplicationQuit()
    {
        //B4 clear, save inventory
        if (inventory.inventoryItems.Count > 0)
        {
            inventory.inventoryItems.Clear();
        }
    }
}
