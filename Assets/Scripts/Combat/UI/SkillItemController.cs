using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverBox;
    [SerializeField] TextMeshProUGUI hoverBoxTitle;
    [SerializeField] TextMeshProUGUI hoverBoxDesc;
    [SerializeField] TextMeshProUGUI hoverBoxManaCost;
    [SerializeField] Image image;
    [SerializeField] GameObject hoverHighlight;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBox.SetActive(true);
        hoverHighlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverBox.SetActive(false);
        hoverHighlight.SetActive(false);
    }

    public void SetSkillTitle(string text)
    {
        hoverBoxTitle.text = text;
    }

    public void SetManaCost(int cost)
    {
        hoverBoxManaCost.text = $"Mana Cost: {cost}";
    }

    public void SetSkillDesc(string desc)
    {
        hoverBoxDesc.text = desc;
    }

    public void SetImageSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
