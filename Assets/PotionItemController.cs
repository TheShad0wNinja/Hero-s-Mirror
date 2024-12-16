using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverBox;
    [SerializeField] TextMeshProUGUI hoverBoxTitle;
    [SerializeField] TextMeshProUGUI hoverBoxDesc;
    [SerializeField] TextMeshProUGUI hoverBoxStats;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI quantity;
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

    public void SetPotionTitle(string text)
    {
        hoverBoxTitle.text = text;
    }

    public void SetPotionImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetHoverDesc(string text)
    {
        hoverBoxDesc.text = text;
    }

    public void SetPotionStats(Dictionary<string, int> stats)
    {
        hoverBoxStats.text = "";
        int count = 0;
        foreach (var stat in stats)
        {
            string headerValue = Regex.Replace(stat.Key, "(\\B[A-Z])", " $1");
            hoverBoxStats.text += $"{headerValue}: {stat.Value}\t\t";
            if (count == 2)
            {
                hoverBoxStats.text += "\n";
            }
            count++;
        }
    }

    public void SetPotionQuantity(int value)
    {
        quantity.text = value.ToString();
    }
}
