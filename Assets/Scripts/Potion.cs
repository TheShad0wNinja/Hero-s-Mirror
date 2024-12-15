using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Potion
{
    Potion_Stats stats = null;
    public Dictionary<string, int> currentStatsFiltered = new Dictionary<string, int>();
    public string name;
    public string description;
    public Sprite image;
    public int price;

    public Potion(Potion_Stats potion)
    {
        this.stats = potion;
        checkValues();
    }
    public void checkValues()
    {
        this.name = stats.name;
        this.description = stats.description;
        this.image = stats.image;
        this.price = stats.price;
        Dictionary<string, int> temp = new Dictionary<string, int>();
        temp.Add("health", stats.health);
        temp.Add("healthRegeneration", stats.healthRegenerationRate);
        temp.Add("mana", stats.mana);
        temp.Add("manaRegeneration", stats.healthRegenerationRate);
        temp.Add("attackBonus", stats.attackBonus);
        temp.Add("shield", stats.shield);
        temp.Add("criticalChance", stats.criticalChance);
        foreach (var item in temp)
        {
            if (item.Value != 0)
            {
                currentStatsFiltered.Add(item.Key, item.Value);
            }
        }
    }
}
