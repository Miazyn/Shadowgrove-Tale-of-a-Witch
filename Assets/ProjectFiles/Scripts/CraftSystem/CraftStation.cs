using System.Collections;
using UnityEngine;


public class CraftStation : MonoBehaviour, IInteractable
{
    [SerializeField] SO_Blueprint[] possibleBlueprintArray;
    Player player;
    SO_Inventory playerInventory;
    public string interactionPrompt => throw new System.NotImplementedException();

    private void Start()
    {
        player = Player.instance;
        playerInventory = player.inventory;
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if(!interactor.TryGetComponent<Player>(out player))
        {
            return false;
            //No Player as interactor
        }

        //For now one recipe
        if (CanCraftItem(0))
        {
            Debug.Log("Going to craft now:" + possibleBlueprintArray[0].BlueprintName);
            return true;
        }

        Debug.Log("Cannot craft yet");
        return false;
    }

    bool CanCraftItem(int index)
    {
        SO_Blueprint currentBlueprint = possibleBlueprintArray[index];
        bool foundItem = false;

        for (int i = 0; i < currentBlueprint.Materials.Length; i++)
        {
            foundItem = false;

            for (int j = 0; j < playerInventory.inventoryItems.Count; j++)
            {
                if (playerInventory.inventoryItems[j].item == currentBlueprint.Materials[i])
                {
                    Debug.Log("Found item in inventory!");
                    foundItem = true;
                }
            }

            if (!foundItem)
            {
                Debug.Log($"Missing the item: {currentBlueprint.Materials[i]}");
                return false;
            }
        }

        Debug.Log("All items for blueprint available.");
        return true;
    }

    ///Needed
    ///- Craft Interface
    ///-> Desc, Mat req, recipe name, result
    ///- Craft Button
    ///- How many Button? -> Increase number/decrease
    ///- Grey out on: Max craftable/can't craft
    ///- Go through inventory to see if you have enough of said material



}
