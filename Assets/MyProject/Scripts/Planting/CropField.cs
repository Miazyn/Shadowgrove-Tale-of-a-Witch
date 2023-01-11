using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string interactionPrompt => prompt;

    public bool HasAPlant;
    [SerializeField] SO_Seed currentPlant;
    int daysToGrow;
    int plantStages;

    Player player;
    private void Start()
    {
        player = Player.instance;
    }

    public bool Interact(Interactor interactor)
    {
        if (HasAPlant)
        {
            return false;
        }
        if (player.GetCurrentItem() == null)
        {
            return false;
        }



        if (player.GetCurrentItem().TypeOfItem == SO_Item.ItemType.Seed)
        {
            Debug.Log($"You just planted some {player.GetCurrentItem().ItemName}." +
                $"Good luck on the growth");
            HasAPlant = true;
            
            currentPlant = (SO_Seed)player.GetCurrentItem();
            daysToGrow = currentPlant.DaysToGrow;
            int counter = 0;
            plantStages = 3;

            StartCoroutine(PlantStages());
            return true;
        }

        
        return false;
    }

    IEnumerator PlantStages()
    {
        int counter = 0;
        while(counter < plantStages)
        {
            yield return new WaitForSeconds(daysToGrow);
            counter++;
            Debug.Log($"{daysToGrow} Days have passed and the plant has grown.");
        }
        Debug.Log($"{currentPlant.ItemName} has fully grown. Now we can harvest.");
    }
}
