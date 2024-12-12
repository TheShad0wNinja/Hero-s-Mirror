using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int weaponID;
    public Sprite weaponSprite;
    public string weaponClass; // "Common", "Rare", "Epic", "Legendary"
    public string weaponDamage;

    public string weaponDescription;
    public string weaponTier;
}
