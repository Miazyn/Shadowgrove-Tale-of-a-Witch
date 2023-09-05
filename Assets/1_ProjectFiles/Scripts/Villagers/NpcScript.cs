using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, IInteractable
{
    public string interactionPrompt => "";

    DialogueManager dialogManager;
    [SerializeField] SO_Dialog _MyDialog;
    [SerializeField] SO_NPC _NPC;

    [SerializeField] private GameObject interactPrompt;
    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

    private void Start()
    {
        dialogManager = DialogueManager.instance;
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interacted with npc");
        dialogManager.SetUpDialog(_MyDialog, _NPC);
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
}
