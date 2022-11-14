using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SO_Inventory playerInventory;


    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ItemHolder>();
        if (item)
        {
            playerInventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        playerInventory.inventoryItems.Clear();
    }
}
