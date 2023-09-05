using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public GameObject InteractPrompt { get; set; }

    public string interactionPrompt { get; }

    public bool Interact(Interactor interactor);

    public bool CanInteract();

    public void ShowInteractPrompt();

    public void HideInteractPrompt();
}
