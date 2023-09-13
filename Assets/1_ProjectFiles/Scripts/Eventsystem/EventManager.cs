using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static UnityEvent OnInteractionStart = new UnityEvent();
    public static UnityEvent OnInteractionEnd = new UnityEvent();

    public static UnityEvent<int,int> OnTimeChanged = new UnityEvent<int, int>();
    public static UnityEvent<int, TimeSystem.Day> OnDayChanged = new UnityEvent<int, TimeSystem.Day>();
    public static UnityEvent<TimeSystem.Season> OnSeasonChanged = new UnityEvent<TimeSystem.Season>();
    public static UnityEvent<int> OnYearChanged = new UnityEvent<int>();
}
