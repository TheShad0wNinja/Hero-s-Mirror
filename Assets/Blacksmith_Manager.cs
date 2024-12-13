using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blacksmith_Manager : MonoBehaviour
{
    //[SerializeField] int commonProbability = 50;
    [SerializeField] int rareProbability = 34;
    [SerializeField] int epicProbability = 10;
    [SerializeField] int legendaryProbability = 5;
    [SerializeField] int mythicProbability = 1;

    [SerializeField] int numOfButtonPressed = 0;
    [SerializeField] GameObject popupPanel;
    private int randomNumber = 0;


    [SerializeField] Image illustrantion;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI rarityText;
    [SerializeField] TextMeshProUGUI statsText;

    public int costPerPurchase = 100;
    public List<Item_Stats> itemScriptables;
    UI_Behaviour_Manager inventoryManager;
    private void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        goldText.text = inventoryManager.gold.ToString();
    }
    public void PurchaseRandomCharacter()
    {
        GenerateRandomItem();
        if (inventoryManager.gold >= costPerPurchase)
        {
            inventoryManager.gold -= costPerPurchase;
            goldText.text = inventoryManager.gold.ToString();
            //newCharacter = null;
        }
        else
        {
            popupPanel.SetActive(true);
        }

    }

    void UpdateItemUI(Item newItem)
    {
        nameText.text = newItem.name;
        illustrantion.sprite = newItem.image;
        rarityText.text = newItem.rarityName;
        statsText.text = "";

        foreach (var stat in newItem.currentStatsFiltered)
        {
            statsText.text += $"{stat.Key}    :    {stat.Value}\n";
        }
    }
    public void GenerateRandomItem()
    {
        numOfButtonPressed++;
        int randomizer = Random.Range(0, itemScriptables.Count);
        Item_Stats newItemScriptable = itemScriptables[randomizer];
        randomNumber = Random.Range(1, 101);
        if (numOfButtonPressed == 1 && randomNumber < 9) 
        {
            randomNumber = Random.Range(10, 21);
        }

        if (randomNumber <= mythicProbability)
        {
            newItemScriptable.rarity = Item_Stats.rarityEnum.Mythical;
        }
        else if (randomNumber <= mythicProbability + legendaryProbability)
        {
            newItemScriptable.rarity = Item_Stats.rarityEnum.Legendary;
        }
        else if (randomNumber <= mythicProbability + legendaryProbability + epicProbability)
        {
            newItemScriptable.rarity = Item_Stats.rarityEnum.Epic;
        }
        else if (randomNumber <= mythicProbability + legendaryProbability + epicProbability + rareProbability)
        {
            newItemScriptable.rarity = Item_Stats.rarityEnum.Rare;
        }
        else
        {
            newItemScriptable.rarity = Item_Stats.rarityEnum.Common;
        }
        Item newItem = new Item(newItemScriptable);
        inventoryManager.AddItem(newItem);
        UpdateItemUI(newItem);
    }
}
