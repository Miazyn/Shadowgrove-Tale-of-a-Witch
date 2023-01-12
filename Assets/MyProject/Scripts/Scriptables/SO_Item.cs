using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Item/Item")]
public class SO_Item : ScriptableObject
{

    public ItemType TypeOfItem;
    public string ItemName;
    [TextArea(15,20)]
    public string ItemDescription;
    public string ItemId;

    public bool CanBeSold;
    public int BuyPrice;
    public int SellPrice;

    public Sprite Icon;

    public enum ItemType
    {
        Item,
        Material,
        Consumable,
        Structure,
        Tools,
        Fish,
        Seed
    }

    public virtual void Use()
    {
        Player.instance.inventory.RemoveItem(this);
    }

    public virtual bool CanBeUsed(GameObject _objectToInteract)
    {
        Debug.Log("Can be used. From Base Class.");
        return true;
    }
}
