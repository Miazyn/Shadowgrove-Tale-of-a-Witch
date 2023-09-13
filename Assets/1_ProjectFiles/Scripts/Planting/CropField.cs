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

    [Header("Plant Stages")]
    [SerializeField] GameObject seedStage;
    [SerializeField] GameObject stage1;
    [SerializeField] GameObject stage2;
    [SerializeField] GameObject stage3;

    private MeshFilter filterStage1;
    private MeshRenderer meshStage1;

    private MeshFilter filterStage2;
    private MeshRenderer meshStage2;

    private MeshFilter filterStage3;
    private MeshRenderer meshStage3;

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

    private void OnEnable()
    {
        EventManager.OnDayChanged.AddListener(GrowPlant);
    }

    private void Start()
    {
        player = Player.instance;
        fieldMesh.material = dryMat;

        filterStage1 = stage1.GetComponent<MeshFilter>();
        meshStage1 = stage1.GetComponent<MeshRenderer>();

        filterStage2 = stage2.GetComponent<MeshFilter>();
        meshStage2 = stage2.GetComponent<MeshRenderer>();

        filterStage3 = stage3.GetComponent<MeshFilter>();
        meshStage3 = stage3.GetComponent<MeshRenderer>();
        watered = false;
    }

    private void OnDisable()
    {
        EventManager.OnDayChanged.RemoveListener(GrowPlant);
    }

    public bool Interact(Interactor interactor)
    {
        if (currentStage == PlantStages.harvestable)
        {
            Harvest();

            return true;
        }

        if (player.IsTool() && currentStage != PlantStages.harvestable)
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

        if (currentStage == PlantStages.planted)
        {
            return false;
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

            if (seedStage != null) seedStage.SetActive(true);

            daysPassed = 1;

            ChangeMeshes();
            //TODO
            return true;
        }
        
        return false;
    }

    private void ChangeMeshes()
    {
        Material[] materials;
        Mesh curmesh;

        if(currentSeed.prefabStage.Length <= 0)
        {
            Debug.LogError("NO MESH FOUND FOR PLANT!");
            return;
        }

        int counter = 0;
        foreach(var stage in currentSeed.prefabStage)
        {
            materials = stage.GetComponent<MeshRenderer>().sharedMaterials;
            curmesh = stage.GetComponent<MeshFilter>().sharedMesh;

            switch (counter)
            {
                case 0:
                    if (filterStage1 != null)
                    {
                        filterStage1.sharedMesh = curmesh;
                    }
                    if (meshStage1 != null)
                    {
                        meshStage1.sharedMaterials = materials;
                    }
                    break;
                case 1:
                    if(filterStage2 != null)
                    {
                        filterStage2.sharedMesh = curmesh;
                    }
                    if(meshStage2 != null)
                    {
                        meshStage2.sharedMaterials = materials;
                    }
                    break;
                case 2:
                    if (filterStage3 != null)
                    {
                        filterStage3.sharedMesh = curmesh;
                    }
                    if (meshStage3 != null)
                    {
                        meshStage3.sharedMaterials = materials;
                    }
                    break;
                default:
                    Debug.Log("Too many stages???");
                    break;
            }

            counter++;
        }
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
        Debug.Log($"Field was watered: {watered}");
        if (!watered) return; 

        int stageintervalls = daysToGrow / 3;

        if(daysPassed >= daysToGrow)
        {
            stage1.SetActive(false);
            stage2.SetActive(false);
            stage3.SetActive(true);

            currentStage = PlantStages.harvestable;

            plant = new Plant(currentSeed.Harvestable, currentSeed.HarvestAmount);
        }
        else
        {
            if (daysPassed > 0)
            {
                if (daysPassed / stageintervalls == 1)
                {
                    if (seedStage != null) seedStage.SetActive(false);
                    stage1.SetActive(true);
                }
                if (daysPassed / stageintervalls == 2)
                {
                    stage1.SetActive(false);
                    stage2.SetActive(true);
                }
            }

            daysPassed++;
        }

        watered = false;
        fieldMesh.material = dryMat;
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
