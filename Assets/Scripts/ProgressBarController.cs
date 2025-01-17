using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public Image middleCurr, endCurr;
    public float endPercentage = 0.2f;

    public void SetBarValue(int curr, int max)
    {
        float endHealth = (int)(0.2 * max);  
        float midHealth = max - endHealth; 

        float endCurrHealth = Mathf.Max(0, curr - midHealth); 
        float midCurrHealth = Mathf.Min(curr, midHealth); 

        float endPercentage = endCurrHealth / endHealth;
        float midPercentage = midCurrHealth / midHealth;

        endCurr.fillAmount = endPercentage;
        middleCurr.fillAmount = midPercentage;
        string barOutput;
        if (curr >= 0) barOutput = $"{curr}/{max}";
        else barOutput = "0";

        barText.text = barOutput;
    }

    internal void Clear()
    {
        SetBarValue(0, 0);
        barText.text = "";
    }
}
