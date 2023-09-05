using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour, IInteractable
{
    public SO_Item item;
    private int amount = 1;
    public string interactionPrompt => throw new System.NotImplementedException();

    [SerializeField] private GameObject interactPrompt;
    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

    Player player;

    private void Start()
    {
        player = Player.instance;

        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>(), true);
    }

    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(Interactor interactor)
    {
        if (player.inventory.AddItem(item, amount))
        {
            Destroy(gameObject);
            return true;
        }
        return false;
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




}
