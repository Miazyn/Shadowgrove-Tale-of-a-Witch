using System.Collections;
using UnityEngine;


public class CraftStation : MonoBehaviour, IInteractable
{
    Player player;
    SO_Inventory playerInventory;
    static string CraftMenu = "CraftMenu";
    public string interactionPrompt => throw new System.NotImplementedException();

    [SerializeField] private GameObject interactPrompt;
    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

    public delegate void OnMenuToggle();
    public OnMenuToggle onMenuTogggleCallback;

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

        GameManager.Instance.onAnyMenuToggleCallback?.Invoke(UIController.Menu.Crafting);

        //onMenuTogggleCallback?.Invoke();
        return true;
    }

    public void Craft(SO_Blueprint _blueprint, int _amount)
    {
        if(_blueprint == null)
        {
            Debug.Log("No blueprint set");
            return;
        }
        if(_amount <= 0)
        {
            Debug.Log("Nothing to craft");
            return;
        }

        if (!CanCraftItem(_blueprint, _amount))
        {
            Debug.Log("Cannot craft yet");
            return;
        }

        Debug.Log("Going to craft now:" + _blueprint.Result);

        for (int i = 0; i < _blueprint.Materials.Length; i++)
        {
            player.inventory.RemoveItems(_blueprint.Materials[i], _amount);
        
        }

        player.inventory.AddItem(_blueprint.Result, _amount);
    }

    bool CanCraftItem(SO_Blueprint _blueprint, int _amount)
    {
        for (int i = 0; i < _blueprint.Materials.Length; i++)
        {
            int requiredAmount = _blueprint.Amount[i] * _amount;

            int availableAmount = 0;

            for (int j = 0; j < playerInventory.inventoryItems.Count; j++)
            {
                if (playerInventory.inventoryItems[j].item == _blueprint.Materials[i])
                {
                    availableAmount += playerInventory.inventoryItems[j].amount;
                }
            }

            if (availableAmount < requiredAmount)
            {
                //Debug.Log($"Missing the item: {_blueprint.Materials[i]} to craft {_amount} items.");
                //Debug.Log($"Player owns: {availableAmount} of {_amount} {_blueprint.Materials[i]} required to craft.");
                return false;
            }
        }

        Debug.Log($"All materials available to craft {_amount} items of blueprint.");
        return true;
    }

    public void ShowInteractPrompt()
    {
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

    ///Needed
    ///- Craft Interface
    ///-> Desc, Mat req, recipe name, result
    ///- Craft Button
    ///- How many Button? -> Increase number/decrease
    ///- Grey out on: Max craftable/can't craft
    ///- Go through inventory to see if you have enough of said material
    ///-  Add possibility for more than one ingredient needed



}
