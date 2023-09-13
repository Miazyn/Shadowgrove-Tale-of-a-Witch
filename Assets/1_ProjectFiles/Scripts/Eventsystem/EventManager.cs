using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static UnityEvent OnInteractionStart = new UnityEvent();
    public static UnityEvent OnInteractionEnd = new UnityEvent();

    public static UnityEvent<int,int> OnTimeChangedInfo = new UnityEvent<int, int>();
    public static UnityEvent<int, TimeSystem.Day> OnDayChangedInfo = new UnityEvent<int, TimeSystem.Day>();
    public static UnityEvent<TimeSystem.Season> OnSeasonChangedInfo = new UnityEvent<TimeSystem.Season>();
    public static UnityEvent<int> OnYearChangedInfo = new UnityEvent<int>();

    public static UnityEvent OnPlayerPassOut = new UnityEvent();
    public static UnityEvent StartNewDay = new UnityEvent();

    public static UnityEvent OnDayChanged = new UnityEvent();
}
