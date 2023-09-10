using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Seed", menuName = "SO/Item/Seed")]
public class SO_Seed : SO_Item
{
    public int DaysToGrow;
    public Season[] GrowthSeason = new Season[1];

    public SO_Item Harvestable;

    public int HarvestAmount;

    [Range(1, 10)]
    public int Stages;
    public Mesh[] MeshForStages;
    public Material[] MeshMaterial;

    //public bool GetSeedsBack;
    //public int SeedMax;
    //public int SeedMin;

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
        Debug.Log($"Planted {ItemName}.");
    }

    public override bool CanBeUsed(GameObject _objectToInteract)
    {
        //Debug.Log("Has to be used with an interactable.");
        return false;
    }
}
