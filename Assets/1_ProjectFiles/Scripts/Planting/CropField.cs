using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string interactionPrompt => prompt;

    Plant plant;

    [Header("Water Status")]
    private bool watered;
    [SerializeField] Material wateredMat;
    [SerializeField] Material dryMat;
    [SerializeField] MeshRenderer fieldMesh;

    [SerializeField] SO_Seed currentSeed;
    int daysToGrow;
    int plantStages;
    int daysPassed = 0;

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
        fieldMesh.material = dryMat;

        watered = false;
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

        if (player.IsTool())
        {
            if (player.GetCurrentItem().ItemName.Contains("Wateringcan"))
            {
                watered = true;

                SO_Tools playerTool = (SO_Tools)player.GetCurrentItem();
                fieldMesh.material = wateredMat;
                player.UseTool(null);

                player.EnduranceChanged(playerTool.GetToolEnduranceUse(SO_Tools.ToolUsage.Proper));
            }
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

    public void GrowPlant()
    {
        if (currentSeed == null) return;

        int stageintervalls = daysToGrow / 3;

        if(daysPassed >= daysToGrow)
        {
            stage1.SetActive(false);
            stage2.SetActive(false);
            stage3.SetActive(true);

            currentStage = PlantStages.harvestable;

            Debug.Log($"{currentSeed.ItemName} has fully grown. Now we can harvest.");
            plant = new Plant(currentSeed.Harvestable, currentSeed.HarvestAmount);
        }
        else
        {
            if(daysPassed/stageintervalls == 1)
            {
                stage1.SetActive(true);
            }
            if(daysPassed/stageintervalls == 2)
            {
                stage1.SetActive(false);
                stage2.SetActive(true);
            }

            daysPassed++;
        }
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
        if(interactionPrompt == null)
        {
            return;
        }
        if(player.GetCurrentItem() == null)
        {
            return;
        }
        
        if (currentSeed == null && player.GetCurrentItem().GetType() == typeof(SO_Seed)|| stage3.activeSelf || player.IsTool() && player.GetCurrentItem().ItemName.Contains("Wateringcan"))
        {
            interactPrompt.SetActive(true);
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
