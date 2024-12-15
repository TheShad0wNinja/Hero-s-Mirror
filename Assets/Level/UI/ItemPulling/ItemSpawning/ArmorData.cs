using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewArmor", menuName = "Armor/Armor Data")]

public class ArmorData : ScriptableObject
{
    // Start is called before the first frame update
    public string armorName;
    public int armorID;
    public Sprite armorSprite;
    public string armorClass; 
    public string armorProtection;

    public string armorDescription;
    public string armorTier; // "Common", "Rare", "Epic", "Legendary"
}
