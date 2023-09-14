using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DescriptionBoxOnHover : MonoBehaviour
{
    [Header("All Text")]
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] RectTransform descriptionWindows;

    public static Action<SO_Item, Vector2> OnMouseOver;
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

    void ShowTip(SO_Item _itemToShow, Vector2 mousePos)
    {
        itemName.text = _itemToShow.ItemName;
        descriptionText.text = _itemToShow.ItemDescription;

        descriptionWindows.gameObject.SetActive(true);
        descriptionWindows.transform.position = new Vector2(mousePos.x - descriptionWindows.sizeDelta.x * 0.35f, mousePos.y);
    }

    void HideTip()
    {
        descriptionText.text = default;
        descriptionWindows.gameObject.SetActive(false);
    }

}
