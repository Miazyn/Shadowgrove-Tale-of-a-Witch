using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int AmountPerHarvest;
    public SO_Item HarvestPlant;

    public Plant(SO_Item _plant, int _amount)
    {
        HarvestPlant = _plant;
        AmountPerHarvest = _amount;
    }
}
