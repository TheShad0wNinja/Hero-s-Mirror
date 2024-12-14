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
    [SerializeField] Image image;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverBox.SetActive(false);
    }

    public void SetSkillTitle(string text)
    {
        hoverBoxTitle.text = text;
    }

    public void SetImageSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
