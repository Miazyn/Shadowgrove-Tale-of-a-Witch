using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "SO/Item/Structure")]
public class SO_Structures : SO_Item
{
    private void Awake()
    {
        TypeOfItem = ItemType.Structure;
        CanBeSold = false;
    }
}
