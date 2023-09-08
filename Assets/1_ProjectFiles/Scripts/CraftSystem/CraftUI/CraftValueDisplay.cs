using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftValueDisplay : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private TMP_InputField input;

    [Header("Info Panel")]
    [SerializeField] private TextMeshProUGUI descriptionBox;
    [SerializeField] private TextMeshProUGUI blueprintName;
    [SerializeField] private GameObject parentRequirements;

    private TextMeshProUGUI[] requiredIngredients;

    private SO_Blueprint curBlueprint = null;
    private int curAmount = 1;

    private CraftStation craftStation;

    private void Awake()
    {
        input.text = "1";
        craftStation = FindObjectOfType<CraftStation>();
    }

    private void Start()
    {
        requiredIngredients = new TextMeshProUGUI[parentRequirements.transform.childCount];

        for(int i = 0; i < parentRequirements.transform.childCount; i++)
        {
            requiredIngredients[i] =  parentRequirements.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        UpdateUI(null);
    }

    public void IncreaseValue()
    {
        if(int.TryParse(input.text, out int result))
        {
            result = 1 + result > 999 ? 999 : 1 + result;
            curAmount = result;

            input.text = result.ToString();
        }
        else
        {
            Debug.Log("Did noz work");
        }
        UpdateUI(curBlueprint);
    }

    public void DecreaseValue() 
    {
        if(int.TryParse(input.text, out int result))
        {
            result = result - 1 < 1 ? 1 : result - 1;
            curAmount = result;

            input.text = result.ToString();
        }
        UpdateUI(curBlueprint);
    }

    public void ValidateInput()
    {
        if(int.TryParse(input.text, out int result))
        {
            result = result > 999 ? 999 : result < 0 ? result * -1 : result;
            curAmount = result;

            input.text = result.ToString();
        }

        UpdateUI(curBlueprint);
    }

    public void UpdateUI(SO_Blueprint _blueprint)
    {
        curBlueprint = _blueprint;

        if(_blueprint == null)
        {
            blueprintName.SetText("Craft Preview");
            descriptionBox.SetText("Click a Blueprint to view details.");

            for (int i = 0; i < requiredIngredients.Length; i++)
            {
                requiredIngredients[i].SetText("");
            }

            return;
        }

        blueprintName.SetText(_blueprint.BlueprintName);
        descriptionBox.SetText(_blueprint.BlueprintDesc);

        for (int i = 0; i < _blueprint.Materials.Length; i++)
        {
            int amount = _blueprint.Amount[i];
            SO_Item item = _blueprint.Materials[i];

            amount = curAmount > 0 ? amount * curAmount : amount;

            requiredIngredients[i].SetText($"0/{amount} {item.ItemName}");
        }

        if (_blueprint.Materials.Length < requiredIngredients.Length)
        {
            for (int i = _blueprint.Materials.Length; i < requiredIngredients.Length; i++)
            {
                requiredIngredients[i].SetText("");
            }
        }

    }

    public void Craft()
    {
        if(craftStation != null)
        {
            craftStation.Craft(curBlueprint, curAmount);
        }
    }
}
