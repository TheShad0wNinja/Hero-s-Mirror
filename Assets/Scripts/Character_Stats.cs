using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]
public class Character_Stats : ScriptableObject
{
    [Header("Item basics")]
    public new string name;
    public string description;
    public Sprite pixelArt;
    public Sprite image;
    public enum rarityEnum { Common = 1, Rare = 2, Epic = 3, Legendary = 4, Mythical = 5};
    public rarityEnum rarity;

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
