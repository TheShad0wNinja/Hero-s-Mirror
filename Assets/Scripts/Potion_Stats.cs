using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Potion", menuName = "Potion")]
public class Potion_Stats : ScriptableObject
{
    [Header("Item basics")]
    public new string name; //new since there already is a name variable in any Object
    public string description;
    public Sprite image;
    public int price;

    [Header("values for Stat buff/debuff")]
    public int health;
    public int healthRegenerationRate;
    public int mana;
    public int manaRegenrationRate;
    public int shield;
    public int attackBonus;
    public int criticalChance;
}
