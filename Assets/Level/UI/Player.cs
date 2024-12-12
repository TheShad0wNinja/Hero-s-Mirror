using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currency = 100;
    public ProbabilityManager shopManager;
    public ItemProbility blacksmithShopManager;
    public CardCharacter cardCharacterDisplay; // Reference to CardCharacter script

    public List<CharacterData> characterlist = new List<CharacterData>();
    public List<WeaponData> weaponlist = new List<WeaponData>();
    public List<ArmorData> armorlist = new List<ArmorData>();

    public currencyDisplay Bank;
    public CardItemDisplay cardWeaponDisplay;
    private int armorOrWeapon = 0; // zero = no yet, 1 - 50 = armor, 51 - 100 = weapon

    public void PurchaseRandomCharacter()
    {

        int characterCost = 10; // Cost of a random character

        if (currency >= characterCost)
        {
            currency -= characterCost;
            CharacterData newCharacter = shopManager.GenerateRandomCharacter();// creates new characters each time
            characterlist.Add(newCharacter);
            cardCharacterDisplay.UpdateCharacterData(newCharacter);
            Debug.Log($"You received: {newCharacter.characterName}");
            Bank.UpdateCurrency();
        }
        else
        {
            Debug.Log("Not enough currency!");
        }

    }

    public void PurchaseRandomItems()
    {

        int weaponCost = 10; // Cost of a random character
        armorOrWeapon = Random.Range(1, 101);// zero = no yet, 1 - 50 = armor, 51 - 100 = weapon

        if (currency >= weaponCost)
        {
            currency -= weaponCost;
            if (armorOrWeapon > 0 && armorOrWeapon < 50) // generates armor 
            {
                ArmorData newArmor = blacksmithShopManager.GenerateRandomArmor();// creates new weapon each time
                armorlist.Add(newArmor);
                Debug.Log($"You received: {newArmor.armorName}");
                cardWeaponDisplay.UpdateItemDisplayData(null, newArmor);

            }
            else if (armorOrWeapon > 51 && armorOrWeapon < 100)// generates weapon
            {
                WeaponData newWeapon = blacksmithShopManager.GenerateRandomWeapon();// creates new weapon each time
                weaponlist.Add(newWeapon);
                Debug.Log($"You received: {newWeapon.weaponName}");
                cardWeaponDisplay.UpdateItemDisplayData(newWeapon, null);
            }

            Bank.UpdateCurrency();
        }
        else
        {
            Debug.Log("Not enough currency!");
        }


    }
}
