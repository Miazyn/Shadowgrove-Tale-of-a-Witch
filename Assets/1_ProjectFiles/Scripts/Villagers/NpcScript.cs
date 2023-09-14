using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, IInteractable
{
    public string interactionPrompt => "";

    DialogueManager dialogManager;

    [SerializeField] SO_Dialog[] dialogs;

    [SerializeField]public SO_NPC _NPC;

    [SerializeField] private GameObject interactPrompt;

    public bool interacted = false;

    Player player;

    public enum FriendshipLevel
    {
        Acquaintance,
        Friend,
        Close_Friend,
        Best_Friend,
    }

    public GameObject InteractPrompt
    {
        get { return interactPrompt; }
        set { interactPrompt = value; }
    }

    private void Start()
    {
        player = Player.instance;
        dialogManager = DialogueManager.instance;

        EventManager.OnDayChanged.AddListener(ResetValues);
    }

    private void OnDisable()
    {
        EventManager.OnDayChanged.RemoveListener(ResetValues);
    }

    public bool CanInteract()
    {
        return true;
    }

    public void ResetValues()
    {
        interacted = false;
    }

    public bool Interact(Interactor interactor)
    {
        dialogManager.SetUpDialog(ChooseRandomDialoge(), _NPC, GetFriendshipLevel());

        if (!interacted)
        {
            player.CheckPlayerStat(this);
            interacted = true;
        }

        return true;
    }

    public SO_Dialog ChooseRandomDialoge()
    {
        return dialogs[Random.Range(0, dialogs.Length)];

        //TODO: Add via friendship level.
    }

    public FriendshipLevel GetFriendshipLevel()
    {
        int level = player.CheckFriendshipLevel(_NPC);

        return level <= 50 ? FriendshipLevel.Acquaintance : level > 0 && level <= 100 ? FriendshipLevel.Friend
            : level > 100 && level <= 200 ? FriendshipLevel.Close_Friend : FriendshipLevel.Best_Friend;
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
