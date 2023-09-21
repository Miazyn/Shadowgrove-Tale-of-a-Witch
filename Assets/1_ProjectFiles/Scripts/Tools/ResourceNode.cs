using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IInteractable, IAttackUnit
{
    Player player;
    SO_Inventory playerInventory;

    [SerializeField] SO_Tools[] ValidTools;
    private SO_Tools playerTool;

    
    [Range(0, 50)][Tooltip("Whether I can mine with my tool via hardness.")]
    [SerializeField] private int resourceHardness = 0;

    private int currentHardness;

    [SerializeField] private SO_Item droppedItem;
    [SerializeField] private int droppedAmount = 0;
    public string interactionPrompt => throw new System.NotImplementedException();

    [SerializeField] private GameObject interactPrompt;
    [Range(0, 50)][Tooltip("How much health the resource node has before breaking")]
    [SerializeField] private int maxHealth;

    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

    public int Health { get; set; }
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public enum ResourceType
    {
        Wood,
        Stone,

    }

    bool hasResources = true;

    public ResourceType resource;

    private void Start()
    {
        if(droppedItem == null)
        {
            Debug.LogError($"No Dropped Item defined for {this.gameObject.name}.");
        }

        //currentHardness = resourceHardness;
        Health = maxHealth;

        player = Player.instance;
        playerInventory = player.inventory;
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (!hasResources)
        {
            return false;
        } 

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
                playerTool = (SO_Tools)player.GetCurrentItem();

                if(playerTool.MaxHardness < resourceHardness)
                {
                    return false;
                }

                continue;
            }
        }

        CollectResource();

        return true;
    }

    public void CollectResource()
    {
        if(droppedItem == null)
        {
            Debug.LogError("Didnt add dropped Item to resource node!!!");
            return;
        }

        TakeDamage(playerTool.Strength);
        
        if (Health == 0) 
        {
            playerInventory.AddItem(droppedItem, droppedAmount);
            Collapse();
        }

        player.UseTool(gameObject);

        player.EnduranceChanged(playerTool.GetToolEnduranceUse(SO_Tools.ToolUsage.Proper));

        //System with any tool can chop, but slower.
        //if (currentHardness >= 0)
        //{
        //    playerInventory.AddItem(droppedItem, droppedAmount);
        //}
        //else
        //{
        //    currentHardness -= playerTool.MaxHardness;
        //}
    }
    public void TakeDamage(int dmg)
    {
        Health = Health - dmg < 0 ? 0 : Health - dmg;
    }

    public void ShowInteractPrompt()
    {
        if (!hasResources)
        {
            return;
        }
        if (InteractPrompt != null)
        {
            InteractPrompt.SetActive(true);
        }
    }

    public void HideInteractPrompt()
    {
        if (InteractPrompt != null)
        {
            InteractPrompt.SetActive(false);
        }
    }

    public void Collapse()
    {
        hasResources = false;
        HideInteractPrompt();
    }

    public void Heal(int healHP)
    {
        Health = maxHealth;
        hasResources = true;
    }
}
