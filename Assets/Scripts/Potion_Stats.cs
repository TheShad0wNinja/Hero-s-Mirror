using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Potion", menuName = "Potion")]
public class Potion_Stats : ScriptableObject
{
    [Header("Item basics")]
    public new string name; //new since there already is a name variable in any Object
    public string description;
    public Sprite pixelArt;
    public Sprite image;
    public int price;

    [Header("values for Stat buff/debuff")]
    public int health;
    public int damage;
    public int armor;
    public int shield;
    public int regeneration;
    public int dodge;
    public int criticalChance;
    public int mana;
}
