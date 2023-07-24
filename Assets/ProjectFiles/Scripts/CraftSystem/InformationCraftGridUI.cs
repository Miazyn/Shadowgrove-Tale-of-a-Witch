using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationCraftGridUI : MonoBehaviour
{

    public SO_Blueprint ItemBlueprint { get; private set; }
    public Image BpImage;
    public TextMeshProUGUI BpName;

    public void SetItemBlueprint(SO_Blueprint _blueprint)
    {
        ItemBlueprint = _blueprint;
        UpdateCraftUI();
    }

    public void UpdateCraftUI()
    {
        if(ItemBlueprint.Result.Icon != null)
        {
            BpImage.sprite = ItemBlueprint.Result.Icon;
        }
        BpName.text = ItemBlueprint.BlueprintName;
    }
}
