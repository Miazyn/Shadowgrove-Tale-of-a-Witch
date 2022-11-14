using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "SO/Item/Tool")]
public class SO_Tools : SO_Item
{
    //For tools with an increasing range
    public bool hasEffectArea;
    public Vector2 effectArea;
    //For Tools which increasingly can hit harder stuff
    public int maxHardness;
    private void Awake()
    {
        itemType = ItemType.Tools;
        CanBeSold = false;
    }
}
