using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool : MonoBehaviour
{
    // Start is called before the first frame update

    public List<CharacterData> commonCharacters;
    public List<CharacterData> rareCharacters;
    public List<CharacterData> epicCharacters;
    public List<CharacterData> legendaryCharacters;
    private int previousCharacterIndex = -1;

    public CharacterData GetRandomCharacter(List<CharacterData> characterList)
    {
        int randomIndex = Random.Range(0, characterList.Count);
         while (randomIndex == previousCharacterIndex)// prevents duplication
        {
            randomIndex = Random.Range(0, characterList.Count);
        }
        previousCharacterIndex = randomIndex;

        return characterList[randomIndex];
    }

}
