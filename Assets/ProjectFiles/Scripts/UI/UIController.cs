using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] public GameObject CraftMenu;
    [SerializeField] public GameObject InventoryMenu;
    [SerializeField] public GameObject Hotbar;

    private GameObject currentMenu;

    public enum Menu
    {
        Crafting,
        Inventory
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
        gamemanager.onMenuClosedCallback += CloseCurrentWindow;

        Hotbar.SetActive(true);
    }

    private void OnDisable()
    {
        gamemanager.onAnyMenuToggleCallback -= AnyMenuToggled;
        gamemanager.onMenuClosedCallback -= CloseCurrentWindow;
    }


    private void AnyMenuToggled(Menu _menu)
    {
        if(currentMenu != null)
        {
            if (currentMenu.activeSelf == true)
            {
                Debug.Log("Current Menu still active, cannot open new menu!");
                return;
            }
        }

        switch (_menu)
        {
            case Menu.Crafting:
                EnableMenu(CraftMenu);
                break;
            case Menu.Inventory:
                EnableMenu(InventoryMenu);

                break;
            default:
                break;
        }
    }

    private void EnableMenu(GameObject _go)
    {
        if(_go == null)
        {
            return;
        }

        _go.SetActive(true);
        currentMenu = _go;

        Hotbar.SetActive(false);

        EventManager.OnInteractionStart?.Invoke();
    }
    private void CloseCurrentWindow()
    {
        if (currentMenu == null)
        {
            return;
        }
        currentMenu.SetActive(false);

        Hotbar.SetActive(true);

        EventManager.OnInteractionEnd?.Invoke();
    }


}
