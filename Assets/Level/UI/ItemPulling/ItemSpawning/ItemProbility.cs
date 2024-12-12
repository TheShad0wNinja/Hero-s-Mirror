using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProbility : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemPool newitemPool;
    //    private int commonProbability = 60;
    private int rareProbability = 20;
    private int epicProbability = 9;
    private int legendaryProbability = 1;
    private int numOfButtonPressed = 0;
    private int previousnumber = 0;


    // Update is called once per frame
    public WeaponData GenerateRandomWeapon()
    {
        numOfButtonPressed++;
        int randomNumber = Random.Range(1, 101); // 1-100
        if (numOfButtonPressed == 1 && randomNumber < 9) // makes sure that on first try player doesn't get high Weapon
        {
            Debug.Log("received high character first time");
            while (randomNumber < 9)
            {
                randomNumber = Random.Range(1, 101);
            }
        }
        previousnumber = randomNumber;

        if (newitemPool.commonWeapon.Count == 0 &&
            newitemPool.rareWeapon.Count == 0 &&
            newitemPool.epicWeapon.Count == 0 &&
            newitemPool.legendaryWeapon.Count == 0)
        {
            Debug.Log("All character lists are empty!");
            return null;
        }

        if (randomNumber <= legendaryProbability)
        {
            return newitemPool.GetRandomWeapon(newitemPool.legendaryWeapon);
        }
        else if (randomNumber <= legendaryProbability + epicProbability)
        {
            return newitemPool.GetRandomWeapon(newitemPool.epicWeapon);
        }
        else if (randomNumber <= legendaryProbability + epicProbability + rareProbability)
        {
            return newitemPool.GetRandomWeapon(newitemPool.rareWeapon);
        }
        else
        {
            return newitemPool.GetRandomWeapon(newitemPool.commonWeapon); 
        }
    }

 public ArmorData GenerateRandomArmor()
    {
        numOfButtonPressed++;
        int randomNumber = Random.Range(1, 101); // 1-100
        if (numOfButtonPressed == 1 && randomNumber < 9) // makes sure that on first try player doesn't get high Weapon
        {
            Debug.Log("received high armor first time");
            while (randomNumber < 9)
            {
                randomNumber = Random.Range(1, 101);
            }
        }
        previousnumber = randomNumber;
Debug.Log("teehee1");
        if (newitemPool.commonArmor.Count == 0 &&
            newitemPool.rareArmor.Count == 0 &&
            newitemPool.epicArmor.Count == 0 &&
            newitemPool.legendaryArmor.Count == 0)
        {
            Debug.Log("All Armor lists are empty!");
            return null;
        }

        if (randomNumber <= legendaryProbability)
        {
            return newitemPool.GetRandomArmor(newitemPool.legendaryArmor);
        }
        else if (randomNumber <= legendaryProbability + epicProbability)
        {
            return newitemPool.GetRandomArmor(newitemPool.epicArmor);
        }
        else if (randomNumber <= legendaryProbability + epicProbability + rareProbability)
        {
            return newitemPool.GetRandomArmor(newitemPool.rareArmor);
        }
        else
        {
            return newitemPool.GetRandomArmor(newitemPool.commonArmor); 
        }
    }

}
