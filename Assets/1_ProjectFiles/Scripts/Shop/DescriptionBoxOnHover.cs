using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DescriptionBoxOnHover : MonoBehaviour
{
    [Header("All Text")]
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Image itemImage;

    public static Action<SO_Item> OnMouseOver;
    public static Action OnMouseLoseFocus;

    void OnEnable()
    {
        OnMouseOver += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    void OnDisable()
    {
        OnMouseOver -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    void Start()
    {
        HideTip();   
    }

    void ShowTip(SO_Item _itemToShow)
    {
        if (_itemToShow == null)
        {
            return;
        }

        if (_itemToShow.ItemName != null)
        {
            itemName.text = _itemToShow.ItemName;
        }

        if (_itemToShow.ItemDescription != null)
        {
            descriptionText.text = _itemToShow.ItemDescription;
        }

        if (_itemToShow.Icon != null)
        {
            itemImage.sprite = _itemToShow.Icon;
        }
    }

    void HideTip()
    {

    }

}
