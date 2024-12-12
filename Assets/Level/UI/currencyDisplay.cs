using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class currencyDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public PurchaseManager PlayerCurrency;
    public TextMeshProUGUI CurrencyDisplay;
    void Awake()
    {
        CurrencyDisplay.text = "$"+ PlayerCurrency.currency.ToString();
    }
    public void UpdateCurrency()
    {
        CurrencyDisplay.text = "$"+ PlayerCurrency.currency.ToString();

    }
}
