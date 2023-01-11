using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Seed", menuName = "SO/Item/Seed")]
public class SO_Seed : SO_Item
{
    public int DaysToGrow;
    public Season[] GrowthSeason = new Season[1];
    public Sprite[] PlantStages;

    public SO_Item Harvestable;

    private void Awake()
    {
        TypeOfItem = ItemType.Seed;
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
