using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Level_UI_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textGold;
    [SerializeField] TextMeshProUGUI name1;
    [SerializeField] TextMeshProUGUI name2;
    [SerializeField] TextMeshProUGUI name3;

    [SerializeField] Image image1;
    [SerializeField] Image image2;
    [SerializeField] Image image3;

    [SerializeField] ProgressBarController healthBar1;
    [SerializeField] ProgressBarController healthBar2;
    [SerializeField] ProgressBarController healthBar3;

    UI_Behaviour_Manager inventoryManager;

    Character ch1, ch2, ch3;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        setValues();
        UpdateUI();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        healthBar1.SetBarValue(ch1.currentHealth, ch1.currentStats["health"]);
        healthBar2.SetBarValue(ch2.currentHealth, ch2.currentStats["health"]);
        healthBar3.SetBarValue(ch3.currentHealth, ch3.currentStats["health"]);
        textGold.text = inventoryManager.gold.ToString();

    }
    void setValues() 
    {
        ch1 = inventoryManager.teamAssembleCharacters[0];
        ch2 = inventoryManager.teamAssembleCharacters[1];
        ch3 = inventoryManager.teamAssembleCharacters[2];
        image1.sprite = ch1.image;
        image2.sprite = ch2.image;
        image3.sprite = ch3.image;
        name1.text = ch1.name;
        name2.text = ch2.name;
        name3.text = ch3.name;
    }
}
