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

    [SerializeField] GameObject stage1;
    [SerializeField] GameObject stage2;
    [SerializeField] GameObject stage3;

    Player player;

    [SerializeField] private GameObject interactPrompt;
    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

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
            Harvest();

            return true;
        }
        if (player.GetCurrentItem() == null)
        {
            return false;
        }


        if (CanInteract())
        {
            currentSeed = (SO_Seed)player.GetCurrentItem();

            currentSeed.Use();

            currentStage = CropField.PlantStages.planted;
            
            daysToGrow = currentSeed.DaysToGrow;
            int counter = 0;
            plantStages = 3;

            HideInteractPrompt();

            StartCoroutine(GrowPlantStages());
            return true;
        }
        
        return false;
    }

    private void Harvest()
    {
        Debug.Log($"Harvested crop: {plant.HarvestPlant.ItemName} x{plant.AmountPerHarvest}");
        player.inventory.AddItem(plant.HarvestPlant, plant.AmountPerHarvest);
        plant = null;

        stage3.SetActive(false);

        currentStage = PlantStages.empty;

        currentSeed = null;

        HideInteractPrompt();
    }

    IEnumerator GrowPlantStages()
    {
        int counter = 0;
        while(counter < plantStages)
        {
            yield return new WaitForSeconds(daysToGrow);
            counter++;
            Debug.Log($"{daysToGrow} Days have passed and the plant has grown.");
            if(counter == 1)
            {
                stage1.SetActive(true);
            }
            if(counter == 2)
            {
                stage1.SetActive(false);
                stage2.SetActive(true);
            }
            if(counter == plantStages)
            {
                stage2.SetActive(false);
                stage3.SetActive(true);
            }
            
        }
        currentStage = PlantStages.harvestable;

        Debug.Log($"{currentSeed.ItemName} has fully grown. Now we can harvest.");
        plant = new Plant(currentSeed.Harvestable, currentSeed.HarvestAmount);
    }

    public bool CanInteract()
    {
        if(player.GetCurrentItem() == null)
        {
            return false;
        }
        if (player.GetCurrentItem().TypeOfItem == SO_Item.ItemType.Seed)
        {
            return true;
        }
        if(currentStage == PlantStages.harvestable)
        {
            return true;
        }
        return false;
    }

    public void ShowInteractPrompt()
    {
        if (InteractPrompt != null)
        {
            if (currentSeed == null || stage3.activeSelf)
            {
                interactPrompt.SetActive(true);
            }
        }
    }

    public void HideInteractPrompt()
    {
        if (InteractPrompt != null)
        {
            InteractPrompt.SetActive(false);
        }
    }
}
