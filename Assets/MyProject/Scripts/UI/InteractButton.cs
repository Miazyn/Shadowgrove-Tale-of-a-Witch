using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class InteractButton : MonoBehaviour
{
    [SerializeField] GameObject pressToButton;
    Interactor interactor;
    PlayerInput playerInput;

    [SerializeField] TextMeshProUGUI buttonText;

    void Start()
    {
        pressToButton.SetActive(false);
        interactor = Player.instance.GetComponent<Interactor>();

        playerInput = Player.instance.GetComponent<PlayerInput>();
    }

    void Update()
    {
        if(interactor.GetInteractable() != null)
        {
            UpdateButtonText();
            Show();
        }
        else
        {
            Hide();
        }
    }

    void UpdateButtonText()
    {
        if(playerInput.currentControlScheme == "Keyboard and Mouse")
        {
            buttonText.SetText("E");
        }
        if(playerInput.currentControlScheme == "Controller")
        {
            buttonText.SetText("O");
        }
    }

    void Show()
    {
        pressToButton.SetActive(true);
    }
    void Hide()
    {
        pressToButton.SetActive(false);
    }
}
