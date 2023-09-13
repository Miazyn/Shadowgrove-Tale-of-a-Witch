using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] public GameObject CraftMenu;
    [SerializeField] public GameObject InventoryMenu;
    [SerializeField] public GameObject Hotbar;

    private GameObject currentMenu;

    private HealthSlider healthbar;
    private EnduranceSlider enduranceBar;
    public enum Menu
    {
        Crafting,
        Inventory
    }

    GameManager gamemanager;

    [Header("Player related")]
    [SerializeField] TextMeshProUGUI moneyText;


    [Header("Time related")]
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI yearText;

    [Header("Weather/Season")]
    [SerializeField] TextMeshProUGUI seasonText;
    [SerializeField] Image weatherIcon;
    [SerializeField] Sprite rainyWeather;
    [SerializeField] Sprite sunnyWeather;
    [SerializeField] Sprite snowyWeather;
    [SerializeField] Image seasonIcon;
    [SerializeField] Sprite spring;
    [SerializeField] Sprite summer;
    [SerializeField] Sprite fall;
    [SerializeField] Sprite winter;

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

        healthbar = FindObjectOfType<HealthSlider>();
        enduranceBar = FindObjectOfType<EnduranceSlider>();
    }

    private void OnEnable()
    {
        EventManager.OnTimeChangedInfo.AddListener(TimeUpdate);
        EventManager.OnDayChangedInfo.AddListener(DayUpdate);
        EventManager.OnSeasonChangedInfo.AddListener(SeasonUpdate);
        EventManager.OnYearChangedInfo.AddListener(YearUpdate);
    }

    private void Start()
    {
        gamemanager = GameManager.Instance;

        gamemanager.onAnyMenuToggleCallback += AnyMenuToggled;
        gamemanager.onMenuClosedCallback += CloseCurrentWindow;
        gamemanager.onPlayerHealthChangeCallback += PlayerTookDamage;
        gamemanager.onPlayerEnduranceChangeCallback += PlayerEnduranceChange;

        Player.instance.onPlayerMoneyChangedCallback += UpdateMoney;

        Hotbar.SetActive(true);
    }

    private void OnDisable()
    {
        gamemanager.onAnyMenuToggleCallback -= AnyMenuToggled;
        gamemanager.onMenuClosedCallback -= CloseCurrentWindow;
        gamemanager.onPlayerHealthChangeCallback -= PlayerTookDamage;
        gamemanager.onPlayerEnduranceChangeCallback -= PlayerEnduranceChange;

        EventManager.OnTimeChangedInfo.RemoveListener(TimeUpdate);
        EventManager.OnDayChangedInfo.RemoveListener(DayUpdate);
        EventManager.OnSeasonChangedInfo.RemoveListener(SeasonUpdate);
        EventManager.OnYearChangedInfo.RemoveListener(YearUpdate);

        Player.instance.onPlayerMoneyChangedCallback -= UpdateMoney;
    }

    private void UpdateMoney(int curMoney)
    {
        moneyText.text = curMoney.ToString() + " G";
    }

    private void TimeUpdate(int minute, int hour) 
    {
        string formattedTime = hour.ToString("D2") + ":" + minute.ToString("D2") + "o`clock";

        clockText.text = formattedTime;
    }

    private void DayUpdate(int daycounter, TimeSystem.Day day) 
    {
        string formattedDay = "";

        switch (day)
        {
            case TimeSystem.Day.Mon:
                formattedDay = daycounter.ToString() + "(Mon)";
                break;
            case TimeSystem.Day.Tue:
                formattedDay = daycounter.ToString() + "(Tue)";

                break;
            case TimeSystem.Day.Wed:
                formattedDay = daycounter.ToString() + "(Wed)";

                break;
            case TimeSystem.Day.Thur:
                formattedDay = daycounter.ToString() + "(Thur)";

                break;
            case TimeSystem.Day.Fri:
                formattedDay = daycounter.ToString() + "(Fri)";

                break;
            case TimeSystem.Day.Sat:
                formattedDay = daycounter.ToString() + "(Sat)";

                break;
            case TimeSystem.Day.Sun:
                formattedDay = daycounter.ToString() + "(Sun)";

                break;
            default:
                break;
        }

        dayText.text = formattedDay;
    }

    private void SeasonUpdate(TimeSystem.Season season) 
    {
        switch (season)
        {
            case TimeSystem.Season.Spring:
                seasonText.text = "Spring";
                seasonIcon.sprite = spring;
                break;
            case TimeSystem.Season.Summer:
                seasonText.text = "Summer";
                seasonIcon.sprite = summer;
                break;
            case TimeSystem.Season.Fall:
                seasonText.text = "Fall";
                seasonIcon.sprite = fall;
                break;
            case TimeSystem.Season.Winter:
                seasonText.text = "Winter";
                seasonIcon.sprite = winter;
                break;
            default:
                break;
        }
    }

    private void YearUpdate(int yearnum) 
    {
        yearText.text = "Year " + yearnum.ToString();
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

        currentMenu = null;
    }

    private void PlayerTookDamage(int maxhealth, int curhealth)
    {
        healthbar.SetMaxValue(maxhealth);
        healthbar.SetValue(curhealth);
    }

    private void PlayerEnduranceChange(int maxendurance, int curendurance)
    {
        enduranceBar.SetMaxValue(maxendurance);
        enduranceBar.SetValue(curendurance);
    }
}
