using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class CardItemDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public WeaponData weaponData;
    public ArmorData armorData;

    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponClass;
    public Image weaponImage;
    public TextMeshProUGUI weaponDamage;
    public TextMeshProUGUI weaponDescription;
    public TextMeshProUGUI weaponTier;
    // armor text
    public TextMeshProUGUI armorName;
    public TextMeshProUGUI armorClass;
    public Image armorImage;
    public TextMeshProUGUI armorDamage;
    public TextMeshProUGUI armorDescription;
    public TextMeshProUGUI armorTier;

    // display text
    public TextMeshProUGUI displayClass;
    public TextMeshProUGUI displayDamageorProtection;
    public TextMeshProUGUI displayTier;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateItemDisplayData(WeaponData newWeaponData, ArmorData newArmorData)
    {
        if (newWeaponData != null)
        {
            // Update the UI with the new character's data
            weaponName.text = newWeaponData.weaponName;
            weaponClass.text = "Class " + newWeaponData.weaponClass;
            weaponImage.sprite = newWeaponData.weaponSprite;
            weaponDamage.text = "Damage " + newWeaponData.weaponDamage.ToString();
            weaponDescription.text = newWeaponData.weaponDescription;
            weaponTier.text = newWeaponData.weaponTier;

            
            displayClass.text = "Weapon Class";
            displayDamageorProtection.text = "Weapon Damage";
            displayTier.text = "Weapon Tier";

        }
        else if (newArmorData != null)
        {
            armorName.text = newArmorData.armorName;
            armorClass.text = "Class " + newArmorData.armorClass;
            armorImage.sprite = newArmorData.armorSprite;
            armorDamage.text = "Protection " + newArmorData.armorProtection.ToString();
            armorDescription.text = newArmorData.armorDescription;
            armorTier.text = newArmorData.armorTier;

            displayClass.text = "Armor Class";
            displayDamageorProtection.text = "Armor Protection";
            displayTier.text = "Armor Tier";
        }
        else
        {
            // Set default values if character data is null
            weaponName.text = "No item";
            weaponClass.text = "Unknown";
            weaponDamage.text = "N/A";

        }
    }

}
