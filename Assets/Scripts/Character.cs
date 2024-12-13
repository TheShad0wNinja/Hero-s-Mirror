using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    Character_Stats stats = null;
    public Dictionary<string, int> currentStats = new Dictionary<string, int>();
    public Dictionary<string, Item> equipedItems = new Dictionary<string, Item>();
    public int xp = 0;
    public int xpRequired = 100;
    public int xpMultiplicationFactor = 2;
    public int Level = 1;
    public string name;
    public string description;
    public Sprite pixelArt;
    public Sprite image;
    public string rarityName;
    public int rarityTier;

    public Character(Character_Stats stats) 
    {
        this.stats = stats;
        CheckValues();
    }
    public void CheckValues()
    {
        currentStats.Add("health", stats.health);
        currentStats.Add("damage", stats.damage);
        currentStats.Add("armor", stats.armor);
        currentStats.Add("shield", stats.shield);
        currentStats.Add("regeneration", stats.regeneration);
        currentStats.Add("dodge", stats.dodge);
        currentStats.Add("criticalChance", stats.criticalChance);
        currentStats.Add("mana", stats.mana);
        this.name = stats.name;
        this.description = stats.description;
        this.pixelArt = stats.pixelArt;
        this.image = stats.image;
        this.rarityName = stats.rarity.ToString();
        this.rarityTier = (int)stats.rarity;
        equipedItems.Add("Helmet", null);
        equipedItems.Add("Armor", null);
        equipedItems.Add("Weapon", null);
        equipedItems.Add("Boot", null);
        equipedItems.Add("Amulet", null);
        recalculateValues();
    }
    public void AddEquippedItem(Item item)
    {
        equipedItems[item.type.ToString()] = item;
        recalculateValues();
    }
    public void RemoveEquippedItem(Item item)
    {
        equipedItems[item.type.ToString()] = null;
        recalculateValues();
    }
    public void mergeXP(int xp)
    {
        this.xp += xp;
        CheckLevel();
    }
    void CheckLevel()
    {
        while (xp >= xpRequired)
        {
            xp -= xpRequired;
            Level++;
            xpRequired *= xpMultiplicationFactor;
        }
        recalculateValues();
    }
    public Item GetEquippedItem(string itemType) 
    {
        return equipedItems[itemType];
    }
    void recalculateValues()
    {
        Dictionary<string, int> updatedStats = new Dictionary<string, int>();

        foreach (var element in currentStats)
        {
            updatedStats[element.Key] = (int)(element.Value * (1 + (Level * 0.1f)) * rarityTier);
        }

        foreach (var element in updatedStats)
        {
            currentStats[element.Key] = element.Value;
        }

        foreach (var element in equipedItems)
        {
            if (element.Value != null)
            {
                foreach (var stat in element.Value.currentStatsFiltered)
                {
                    currentStats[stat.Key] += stat.Value;
                }
            }
        }
    }

}
