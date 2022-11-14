using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "SO/Item/Material")]
public class SO_Material : SO_Item
{
    private void Awake()
    {
        itemType = ItemType.Material;
        CanBeSold = true;
    }
}
