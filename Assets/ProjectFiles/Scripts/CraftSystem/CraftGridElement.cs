using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftGridElement : MonoBehaviour
{

    public SO_Blueprint ItemBlueprint { get; private set; }
    public Image BpImage;
    public Image BpBackground;
    public TextMeshProUGUI BpName;

    public int Index { get; private set; }

    public bool Craftable { get; private set; }
    [SerializeField] private Color Disabled;

    private CraftValueDisplay CraftValues;

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

        if (Craftable)
        {
            BpImage.color = Color.white;
            BpBackground.color = Color.white;
        }
        else
        {
            BpImage.color = Disabled;
            BpBackground.color = Disabled;
        }
    }

    public void SetCraftable(bool _craftable)
    {
        Craftable = _craftable;
    }

    public void SetIndex(int _index)
    {
        Index = _index;
    }

    public void OnClickEvent()
    {
        if(CraftValues == null)
        {
            CraftValues = GameObject.FindObjectOfType<CraftValueDisplay>();
        }

        CraftValues.UpdateUI(ItemBlueprint);
    }
}
