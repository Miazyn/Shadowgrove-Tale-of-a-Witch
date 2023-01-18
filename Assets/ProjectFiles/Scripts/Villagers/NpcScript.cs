using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, IInteractable
{
    public string interactionPrompt => "";

    DialogueManager dialogManager;
    [SerializeField] SO_Dialog _MyDialog;
    [SerializeField] SO_NPC _NPC;

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
        dialogManager.SetUpDialog(_MyDialog, _NPC);
        return true;
    }
}
