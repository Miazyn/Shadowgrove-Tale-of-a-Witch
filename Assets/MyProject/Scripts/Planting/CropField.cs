using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string interactionPrompt => prompt;

    Plant plant;

    [SerializeField] SO_Seed currentSeed;
    int daysToGrow;
    int plantStages;

    Player player;

    public enum PlantStages
    {
        empty,
        planted,
        harvestable
    }
    PlantStages currentStage;

    private void Start()
    {
        player = Player.instance;
    }

    public bool Interact(Interactor interactor)
    {
        if (currentStage == PlantStages.planted)
        {
            return false;
        }
        if(currentStage == PlantStages.harvestable)
        {
            Debug.Log($"Harvested crop: {plant.HarvestPlant.ItemName} x{plant.AmountPerHarvest}");
            player.inventory.AddItem(plant.HarvestPlant, plant.AmountPerHarvest);
            plant = null;

            currentStage = PlantStages.empty;

            return true;
        }
        if (player.GetCurrentItem() == null)
        {
            return false;
        }


        if (player.GetCurrentItem().TypeOfItem == SO_Item.ItemType.Seed)
        {
            currentSeed = (SO_Seed)player.GetCurrentItem();

            currentSeed.Use();


            currentStage = CropField.PlantStages.planted;
            
            daysToGrow = currentSeed.DaysToGrow;
            int counter = 0;
            plantStages = 3;

            StartCoroutine(GrowPlantStages());
            return true;
        }

        
        return false;
    }

    IEnumerator GrowPlantStages()
    {
        int counter = 0;
        while(counter < plantStages)
        {
            yield return new WaitForSeconds(daysToGrow);
            counter++;
            Debug.Log($"{daysToGrow} Days have passed and the plant has grown.");
        }
        currentStage = PlantStages.harvestable;

        Debug.Log($"{currentSeed.ItemName} has fully grown. Now we can harvest.");
        plant = new Plant(currentSeed.Harvestable, currentSeed.HarvestAmount);
    }
}
