using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Behaviour_Manager : MonoBehaviour
{
    public static UI_Behaviour_Manager Instance { get; private set; } // Singleton instance

    public Dictionary<Potion, int> ownedPotions = new Dictionary<Potion, int>();

    public List<Character_Stats> ownedCharactersStats = new List<Character_Stats>();
    public List<Character> ownedCharacters = new List<Character>();

    public List<Item_Stats> ownedItemsStats = new List<Item_Stats>();
    public List<Item> ownedItems = new List<Item>();

    public List<Character> teamAssembleCharacters = new List<Character>();

    public int gold = 10000;

    private void Awake()
    {
        // Ensure there's only one instance of this manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes
            CreateCharacters();
            CreateItems();
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    void CreateCharacters() // temp till character structure is defined
    {
        foreach (var element in ownedCharactersStats)
        {
            ownedCharacters.Add(new Character(element));
        }
    }

    void CreateItems() // temp till item structure is defined
    {
        foreach (var element in ownedItemsStats)
        {
            Debug.Log(element.name); 
            ownedItems.Add(new Item(element));
        }
    }

    public void AddPotion(Potion potion)
    {
        if (ownedPotions.ContainsKey(potion))
        {
            ownedPotions[potion]++;
        }
        else
        {
            ownedPotions.Add(potion, 1);
        }
    }
    public void AddCharacter(Character character)
    {
        ownedCharacters.Add(character);
    }
    public void AddTeamCharacters(List<Character> characters)
    {
        teamAssembleCharacters = characters;
    }
    public void SetGold(int gold) 
    {
        this.gold = gold;
    }
    public int GetGold()
    {
        return gold;
    }
}
