using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    TimeSystem time;

    [SerializeField] private GameObject interactPrompt;
    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

    public string interactionPrompt => throw new System.NotImplementedException();

    Player player;

    private void Start()
    {
        player = Player.instance;
        time = FindObjectOfType<TimeSystem>();
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
        Debug.Log("Time to sleep");
        time.NextDay();
        return true;
    }

    public void ShowInteractPrompt()
    {
        if (interactPrompt != null) interactPrompt.SetActive(true);
    }
    public void HideInteractPrompt()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

}
