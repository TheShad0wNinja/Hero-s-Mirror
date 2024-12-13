using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[Serializable]
public class Potion
{
    Potion_Stats stats = null;
    public Dictionary<string, int> currentStatsFiltered = new Dictionary<string, int>();
    public string name;
    public string description;
    public Sprite pixelArt;
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
        this.pixelArt = stats.pixelArt;
        this.image = stats.image;
        this.price = stats.price;
        Dictionary<string, int> temp = new Dictionary<string, int>();
        temp.Add("health", stats.health);
        temp.Add("damage", stats.damage);
        temp.Add("armor", stats.armor);
        temp.Add("shield", stats.shield);
        temp.Add("regeneration", stats.regeneration);
        temp.Add("dodge", stats.dodge);
        temp.Add("criticalChance", stats.criticalChance);
        temp.Add("mana", stats.mana);
        foreach (var item in temp)
        {
            if (item.Value != 0)
            {
                currentStatsFiltered.Add(item.Key, item.Value);
            }
        }
    }
}
