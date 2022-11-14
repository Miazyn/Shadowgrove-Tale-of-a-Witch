using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "SO/Item/Fish")]
public class SO_Fish : SO_Item
{
    private void Awake()
    {
        itemType = ItemType.Fish;
        CanBeSold = true;
    }
}
