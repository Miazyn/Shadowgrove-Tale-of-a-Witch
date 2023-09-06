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
    private SO_Blueprint currentBlueprint;

    private CraftStation craftStation;

    private void Awake()
    {
        input.text = "0";
        
    }

    private void Start()
    {
        requiredIngredients = new TextMeshProUGUI[parentRequirements.transform.childCount];

        for(int i = 0; i < parentRequirements.transform.childCount; i++)
        {
            requiredIngredients[i] =  parentRequirements.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        craftStation = FindObjectOfType<CraftStation>();

        UpdateUI(null);
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
            Debug.Log("Did not work");
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

    public void UpdateUI(SO_Blueprint _blueprint)
    {
        if(_blueprint == null)
        {
            currentBlueprint = null;

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

            requiredIngredients[i].SetText($"0/{amount} {item.ItemName}");
        }

        if (_blueprint.Materials.Length < requiredIngredients.Length)
        {
            for (int i = _blueprint.Materials.Length; i < requiredIngredients.Length; i++)
            {
                requiredIngredients[i].SetText("");
            }
        }
        currentBlueprint = _blueprint;
    }

    public void Craft()
    {
        if(craftStation == null)
        {
            Debug.LogError("Could not find a crafting station!");
            return;
        }
        if(currentBlueprint == null)
        {
            return;
        }

        craftStation.Craft(currentBlueprint);
        Debug.Log($"Ey mate I am crafting here. {currentBlueprint}");
        
    }

}
