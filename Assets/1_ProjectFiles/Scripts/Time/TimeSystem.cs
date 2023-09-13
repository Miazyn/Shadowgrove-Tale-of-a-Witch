using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeSystem : MonoBehaviour
{
    /// <summary>
    /// Days per Month: 28 || 30
    /// 24 Hrs System
    /// 10 min = 7 seconds
    /// 1 hr = 42 seconds
    /// 24 hrs = 16:48 minutes
    /// </summary>/// 

    [SerializeField] private Day curDay;
    [SerializeField] private Season curSeason;
    [SerializeField] public int Year = 1;

    private int dayCounter = 1;

    private int hour = 6;
    private int minute = 0;

    [Range(0.001f, 10.0f)]
    [SerializeField] private float timeBtwMinutes = 7.0f;
    [Range(1, 31)]
    [SerializeField] private int dayInSeason = 28;
    [Range(0, 23)]
    [SerializeField] private int startHour = 6;
    [Range(0, 23)]
    [SerializeField] private int passOutTime = 2;

    public enum Day
    {
        Mon,
        Tue,
        Wed,
        Thur,
        Fri,
        Sat,
        Sun
    }

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public void Start()
    {

        hour = startHour;

        curDay = Day.Mon;
        curSeason = Season.Spring;

        EventManager.OnTimeChangedInfo?.Invoke(minute, hour);
        EventManager.OnDayChangedInfo?.Invoke(dayCounter, curDay);
        EventManager.OnSeasonChangedInfo?.Invoke(curSeason);
        EventManager.OnYearChangedInfo?.Invoke(Year);

        StartCoroutine(HourIncrease());
    }

    public IEnumerator HourIncrease()
    {
        

        while (hour != 2)
        {
            yield return new WaitForSeconds(timeBtwMinutes);

            if (minute + 10 > 50)
            {
                minute = 0;
                if (hour > 22)
                {
                    hour = 0;
                }
                else if (hour != passOutTime)
                {
                    hour++;
                }
            }
            else
            {
                if (hour != passOutTime)
                {
                    minute += 10;
                }
            }

            EventManager.OnTimeChangedInfo?.Invoke(minute, hour);

            //string time = "Time" + hour.ToString("D2") + ":" + minute.ToString("D2") + " Day:" + dayCounter;
        }

        Debug.Log("We have passeed out. Restart Coroutine, next day.");
        NextDay();
    }

    public void NextDay()
    {
        switch (curDay)
        {
            case Day.Mon:
                curDay = Day.Tue;
                break;
            case Day.Tue:
                curDay = Day.Wed;
                break;
            case Day.Wed:
                curDay = Day.Thur;
                break;
            case Day.Thur:
                curDay = Day.Fri;
                break;
            case Day.Fri:
                curDay = Day.Sat;
                break;
            case Day.Sat:
                curDay = Day.Sun;
                break;
            case Day.Sun:
                curDay = Day.Mon;
                break;
            default:
                Debug.LogError("Went outside of the expected day range!!!");
                break;
        }

        if(dayCounter + 1 < dayInSeason + 1)
        {
            dayCounter++;
        }
        else
        {
            dayCounter = 1;

            NextSeason();
        }

        Debug.Log("Next day:" + curDay + " " + dayCounter + " " + curSeason + " Year: " + Year);

        EventManager.OnDayChangedInfo?.Invoke(dayCounter, curDay);

        EventManager.OnDayChanged?.Invoke();

        hour = startHour;
        minute = 0;

        StartCoroutine(HourIncrease());
    }

    public void NextSeason()
    {
        switch (curSeason)
        {
            case Season.Spring:
                curSeason = Season.Summer;
                break;
            case Season.Summer:
                curSeason = Season.Fall;
                break;
            case Season.Fall:
                curSeason = Season.Winter;
                break;
            case Season.Winter:
                curSeason = Season.Spring;
                NextYear();

                break;
            default:
                Debug.LogError("Went outside of the expected possible seasons!!!");
                break;
        }

        EventManager.OnSeasonChangedInfo?.Invoke(curSeason);
    }

    public void NextYear()
    {
        Year++;
        EventManager.OnYearChangedInfo?.Invoke(Year);
    }
}
