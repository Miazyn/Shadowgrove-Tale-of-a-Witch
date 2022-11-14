using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "SO/Item/Consumable")]
public class SO_Consumable : SO_Item
{
    public int restoreHealthValue;
    public int restoreEnduranceValue;
    private void Awake()
    {
        itemType = ItemType.Consumable;
        CanBeSold = true;
    }
}
