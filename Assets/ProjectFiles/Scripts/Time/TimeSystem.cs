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

    [SerializeField] private float timeBtwMinutes = 7f;

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
        curDay = Day.Mon;
        curSeason = Season.Spring;

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
                else if (hour != 2)
                {
                    hour++;
                }
            }
            else
            {
                if (hour != 2)
                {
                    minute += 10;
                }
            }

            //string time = "Time" + hour.ToString("D2") + ":" + minute.ToString("D2") + " Day:" + dayCounter;
            string time = "Time" + hour.ToString("D2") + " Day:" + dayCounter;
            Debug.Log(time);
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

        Debug.Log("Next day:" + curDay);

        dayCounter = dayCounter++ > 27 ? 1 : dayCounter++;

        hour = 6;
        minute = 0;
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
                break;
            default:
                Debug.LogError("Went outside of the expected possible seasons!!!");
                break;
        }
    }

    public void NextYear()
    {
        Year++;
    }
}
