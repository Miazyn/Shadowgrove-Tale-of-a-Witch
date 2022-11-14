using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Useable", menuName = "SO/Item/Useable")]
public class SO_Useable : SO_Item
{
    private void Awake()
    {
        itemType = ItemType.Useable;
        CanBeSold = true;
    }
}
