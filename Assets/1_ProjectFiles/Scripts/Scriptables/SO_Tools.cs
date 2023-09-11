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

    public int Strength;

    public enum ToolUsage
    {
        Proper,
        Improper,
        Waterrefil
    }
    private void Awake()
    {
        TypeOfItem = ItemType.Tools;
        CanBeSold = false;
    }

    public override void Use()
    {
        base.Use();
        Debug.Log($"Used a {ItemName} as a tool");
    }

    public override bool CanBeUsed(GameObject _objectToInteract)
    {
        return base.CanBeUsed(_objectToInteract);
    }

    public int GetToolEnduranceUse(ToolUsage usage)
    {
        if(usage == ToolUsage.Waterrefil)
        {
            return -2;
        }

        if(usage == ToolUsage.Improper)
        {
            return -1;
        }

        switch (MaxHardness)
        {
            case 0:
                return -5;
            case 5:
                return -4;
            case 10:
                return -3;
            case 20:
                return -2;
            case 30:
                return -1;
            default:
                return -1;
        }
    }
}
