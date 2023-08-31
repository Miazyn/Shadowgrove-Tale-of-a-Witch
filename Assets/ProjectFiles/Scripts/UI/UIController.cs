using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public GameObject CraftMenu;

    public enum Menu
    {
        Crafting
    }

    GameManager gamemanager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gamemanager = GameManager.Instance;

        gamemanager.onAnyMenuToggleCallback += AnyMenuToggled;
    }

    private void AnyMenuToggled(Menu _menu)
    {
        if(Menu.Crafting == _menu)
        {
            Toggle(CraftMenu);
            Debug.Log("Toggle Craft Menu");
        }
    }

    private void Toggle(GameObject _toggler)
    {
        if(_toggler == null)
        {
            return;
        }

        _toggler.SetActive(_toggler.activeSelf ? false : true);
    }
}
