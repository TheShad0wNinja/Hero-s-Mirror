using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public string characterClass;
    public int characterID;
    public Sprite characterSprite;
    public int damage;
    public string tier; // "Common", "Rare", "Epic", "Legendary"
}
