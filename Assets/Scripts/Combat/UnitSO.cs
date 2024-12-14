using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Unit")]
public class UnitSO : ScriptableObject
{
    [Header("Attributes")]
    public new string name;
    public string description;
    public Sprite pixelArt;
    public bool flipped;
    public rarityEnum rarity;
    public bool isEnemy;

    [Header("Stats")]
    public int health;
    public int healthRegenerationRate;
    public int mana;
    public int manaRegenrationRate;
    public int shield;
    public int attackBonus = 100;
    public int baseCritChance = 1;

    [Header("Animation")]
    public bool hasHitAnimation;
}

public enum rarityEnum
{
    Common = 1, Rare = 2, Epic = 3, Legendary = 4, Mythical = 5
};