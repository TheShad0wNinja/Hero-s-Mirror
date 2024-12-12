using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityManager : MonoBehaviour
{
    public CharacterPool characterPool;
    //    private int commonProbability = 60;
    private int rareProbability = 20;
    private int epicProbability = 9;
    private int legendaryProbability = 1;
    private int numOfButtonPressed = 0;
    private int previousnumber = 0;
private int randomNumber = 0;
    public CharacterData GenerateRandomCharacter()
    {
        numOfButtonPressed++;
         randomNumber = Random.Range(1, 101); // 1-100
        if (numOfButtonPressed == 1 && randomNumber < 9) // makes sure that on first try player doesnt get high characters
        {
           while (randomNumber < 9)
            {
                randomNumber = Random.Range(1, 101);
            }
        }
        previousnumber = randomNumber;
        if (characterPool.commonCharacters.Count == 0 &&
               characterPool.rareCharacters.Count == 0 &&
               characterPool.epicCharacters.Count == 0 &&
               characterPool.legendaryCharacters.Count == 0)
        {
            Debug.Log("All character lists are empty!");
            return null;
        }

        if (randomNumber <= legendaryProbability)
        {

            return characterPool.GetRandomCharacter(characterPool.legendaryCharacters);
        }
        else if (randomNumber <= legendaryProbability + epicProbability)
        {
            return characterPool.GetRandomCharacter(characterPool.epicCharacters);
        }
        else if (randomNumber <= legendaryProbability + epicProbability + rareProbability)
        {
            return characterPool.GetRandomCharacter(characterPool.rareCharacters);
        }
        else
        {
            return characterPool.GetRandomCharacter(characterPool.commonCharacters);
        }

    }
}
