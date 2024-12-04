using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverBox;
    [SerializeField] TextMeshProUGUI hoverBoxTitle;

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
}
