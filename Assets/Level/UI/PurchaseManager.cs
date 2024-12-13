using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
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
    public List<Character_Stats> characterScriptables;
    UI_Behaviour_Manager inventoryManager;
    private void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        goldText.text = inventoryManager.gold.ToString();
    }
    public void PurchaseRandomCharacter()
    {
        GenerateRandomCharacter();
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

    void UpdateCharacterUI(Character newCharacter) 
    {
        nameText.text = newCharacter.name;
        illustrantion.sprite = newCharacter.image;
        rarityText.text = newCharacter.rarityName;
        statsText.text = "";

        foreach (var stat in newCharacter.currentStats)
        {
            statsText.text += $"{stat.Key}    :    {stat.Value}\n";
        }
    }
    public void GenerateRandomCharacter()
    {
        numOfButtonPressed++;
        int randomizer = Random.Range(0, characterScriptables.Count);
        Character_Stats newCharacterScriptable = characterScriptables[randomizer];
        randomNumber = Random.Range(1, 101); // 1-100
        if (numOfButtonPressed == 1 && randomNumber < 9) // makes sure that on first try player doesnt get high characters
        {
            randomNumber = Random.Range(10, 21);
        }

        if (randomNumber <= mythicProbability)
        {
            newCharacterScriptable.rarity = Character_Stats.rarityEnum.Mythical;
        }
        else if (randomNumber <= mythicProbability + legendaryProbability)
        {
            newCharacterScriptable.rarity = Character_Stats.rarityEnum.Legendary;
        }
        else if (randomNumber <= mythicProbability + legendaryProbability + epicProbability)
        {
            newCharacterScriptable.rarity = Character_Stats.rarityEnum.Epic;
        }
        else if (randomNumber <= mythicProbability + legendaryProbability + epicProbability + rareProbability)
        {
            newCharacterScriptable.rarity = Character_Stats.rarityEnum.Rare;
        }
        else 
        {
            newCharacterScriptable.rarity = Character_Stats.rarityEnum.Common;
        }
        Character newCharacter = new Character(newCharacterScriptable);
        inventoryManager.AddCharacter(newCharacter);
        UpdateCharacterUI(newCharacter);
    }
}
