using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_Item : ScriptableObject
{

    public ItemType itemType;
    public string itemName;
    [TextArea(15,20)]
    public string itemDescription;
    public string itemId;

    public bool CanBeSold;
    public int buyPrice;
    public int sellPrice;

    public Sprite icon;

    public enum ItemType
    {
        Material,
        Consumable,
        Useable,
        Structure,
        Tools,
        Fish
    }

    public virtual void Use()
    {
        Debug.Log("Is using item " + name);
    }
}
