using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    /// <summary>
    /// Days per Month: 28 || 30
    /// 24 Hrs System
    /// 10 min = 7 seconds
    /// 1 hr = 42 seconds
    /// 24 hrs = 16:48 minutes
    /// </summary>
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

}
