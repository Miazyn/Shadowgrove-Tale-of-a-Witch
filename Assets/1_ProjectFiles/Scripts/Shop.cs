using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject InteractPrompt { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public string interactionPrompt => throw new System.NotImplementedException();
    Player player;

    private void Start()
    {
        player = Player.instance;
    }
    public bool CanInteract()
    {
        return true;
    }

    public void HideInteractPrompt()
    {
    }

    public bool Interact(Interactor interactor)
    {
        if (!interactor.TryGetComponent<Player>(out player))
        {
            return false;
            //No Player as interactor
        }

        GameManager.Instance.onAnyMenuToggleCallback?.Invoke(UIController.Menu.Shop);

        return true;
    }

    public void ShowInteractPrompt()
    {
    }
}
