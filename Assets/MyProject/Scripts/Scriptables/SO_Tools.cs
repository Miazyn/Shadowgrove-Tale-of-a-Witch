using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "SO/Item/Tool")]
public class SO_Tools : SO_Item
{
    //For tools with an increasing range
    public bool HasEffectArea;
    public Vector2 EffectArea;
    //For Tools which increasingly can hit harder stuff
    public int MaxHardness;
    private void Awake()
    {
        TypeOfItem = ItemType.Tools;
        CanBeSold = false;
    }
}
