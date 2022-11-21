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


    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ItemHolder>();
        if (item)
        {
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
                Destroy(other.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        //B4 clear, save inventory
        playerInventory.inventoryItems.Clear();
    }
}
