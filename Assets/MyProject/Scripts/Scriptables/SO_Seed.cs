using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Seed", menuName = "SO/Item/Seed")]
public class SO_Seed : SO_Item
{
    public int daysToGrow;
    public Season[] season = new Season[1];
    public Sprite[] PlantStages;

    private void Awake()
    {
        itemType = ItemType.Seed;
        CanBeSold = true;
    }

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public override void Use()
    {
        base.Use();
    }
}
