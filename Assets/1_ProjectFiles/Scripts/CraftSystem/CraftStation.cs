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

    public void Craft(SO_Blueprint _blueprint)
    {
        if (!CanCraftItem(_blueprint))
        {
            Debug.Log("Cannot craft yet");
            return;
        }

        Debug.Log("Going to craft now:" + _blueprint.BlueprintName);
    }

    bool CanCraftItem(SO_Blueprint _blueprint)
    {
        bool foundItem = false;

        for (int i = 0; i < _blueprint.Materials.Length; i++)
        {
            foundItem = false;

            for (int j = 0; j < playerInventory.inventoryItems.Count; j++)
            {
                if (playerInventory.inventoryItems[j].item == _blueprint.Materials[i])
                {
                    Debug.Log("Found item in inventory!");
                    foundItem = true;
                }
            }

            if (!foundItem)
            {
                Debug.Log($"Missing the item: {_blueprint.Materials[i]}");
                return false;
            }
        }

        Debug.Log("All items for blueprint available.");
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
