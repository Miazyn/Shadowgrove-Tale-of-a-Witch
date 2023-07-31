using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldCraft : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;

    private void Awake()
    {
        input.text = "0";
    }

    //Update all Values of each CraftingItem
    public void UpdatingValues()
    {

    }

    public void IncreaseValue()
    {
        if(int.TryParse(input.text, out int result))
        {
            result = 1 + result > 999 ? 999 : 1 + result;
            input.text = result.ToString();
        }
        else
        {
            Debug.Log("Did noz work");
        }
    }

    public void DecreaseValue() 
    {
        if(int.TryParse(input.text, out int result))
        {
            result = result - 1 < 0 ? 0 : result - 1;
            input.text = result.ToString();
        }
    }

    public void ValidateInput()
    {
        if(int.TryParse(input.text, out int result))
        {
            result = result > 999 ? 999 : result < 0 ? result * -1 : result;
            input.text = result.ToString();
        }
    }
}
