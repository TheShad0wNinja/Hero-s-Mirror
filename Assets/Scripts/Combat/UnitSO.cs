using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Unit")]
public class UnitSO : ScriptableObject
{
    [Header("Attributes")]
    public string unitName;
    public Sprite sprite;
    public bool flipped;
    public bool isEnemy;

    [Header("Stats")]
    public int baseMana;
    public int maxMana;
    public int baseHealth; 
    public int maxHealth; 
    public int baseShield = 0;
    public float baseAttackBonus = 1;
    public float baseCritChance = 0.1f;
}
