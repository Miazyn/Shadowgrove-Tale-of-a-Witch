using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IInteractable
{
    Player player;
    SO_Inventory playerInventory;

    [SerializeField] SO_Tools[] ValidTools;
    [Range(0, 50)]
    [SerializeField] private int resourceHardness = 0;

    [SerializeField] private SO_Item droppedItem;
    [SerializeField] private int droppedAmount = 0;
    public string interactionPrompt => throw new System.NotImplementedException();


    public enum ResourceType
    {
        Wood,
        Stone,

    }

    public ResourceType resource;

    private void Start()
    {
        if(droppedItem == null)
        {
            Debug.LogError($"No Dropped Item defined for {this.gameObject.name}.");
        }

        player = Player.instance;
        playerInventory = player.inventory;
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (!interactor.TryGetComponent<Player>(out player))
        {
            return false;
            //No Player as interactor
        }

        if(player.GetCurrentItem() as SO_Tools == null)
        {
            Debug.Log("Not a tool to chop with.");
            return false;
        }

        foreach(var tool in ValidTools)
        {
            if(player.GetCurrentItem() == tool)
            {
                SO_Tools playerTool = (SO_Tools)player.GetCurrentItem();
                if (playerTool.MaxHardness < resourceHardness)
                {
                    Debug.Log("Not strong enough!");
                    return false;
                }
                continue;
            }
        }

        Debug.Log("I am a resource Node");
        CollectResource();

        return true;
    }

    public void CollectResource()
    {
        if(droppedItem == null)
        {
            Debug.Log("DIdnt add dropped Item!!!");
            return;
        }

        playerInventory.AddItem(droppedItem, droppedAmount);

        Debug.Log("Chop chop, tree gone and added items");
    }
}
