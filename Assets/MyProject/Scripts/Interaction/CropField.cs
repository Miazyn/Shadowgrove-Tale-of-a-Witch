using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string interactionPrompt => prompt;

    public enum FieldStates
    {
        flat,
        tilled,
        planted
    }



    Player player;
    private void Start()
    {
        player = Player.instance;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interacted with CropField");
        if (player.GetCurrentItem() == null)
        {
            Debug.Log("Player has currently no item equipped.");
            return false;
        }

        if (player.GetCurrentItem().itemType == SO_Item.ItemType.Seed)
        {
            Debug.Log("You have a seed to use owo");
            return true;
        }

        
        return false;
    }
}
