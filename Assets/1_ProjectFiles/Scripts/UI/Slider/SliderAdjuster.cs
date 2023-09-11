using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAdjuster : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int _maxValue)
    {
        slider.maxValue = _maxValue;
    }

    public void SetValue(int _value)
    {
        slider.value = _value;
    }
}
