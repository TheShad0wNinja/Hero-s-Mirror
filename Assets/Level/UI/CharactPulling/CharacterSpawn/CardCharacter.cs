using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterData characterData;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterClass;
    public Image CharacterImage;
    public TextMeshProUGUI CharacterTier;
    public TextMeshProUGUI CharacterDamage;
    public PurchaseManager newCharacters;

    void Awake()
    {
    }

    // Update is called once per frame
  

    public void UpdateCharacterData(CharacterData newCharacterData)
    {
        if (newCharacterData != null)
        {
            // Update the UI with the new character's data
            characterName.text = newCharacterData.characterName;
            characterClass.text = newCharacterData.characterClass;
            CharacterImage.sprite = newCharacterData.characterSprite;
            CharacterDamage.text = newCharacterData.damage.ToString();
            CharacterTier.text = newCharacterData.tier;
        }
        else
        {
            // Set default values if character data is null
            characterName.text = "No Character";
            characterClass.text = "Unknown";
            CharacterDamage.text = "N/A";
            CharacterTier.text = "N/A";
        }
    }
}
