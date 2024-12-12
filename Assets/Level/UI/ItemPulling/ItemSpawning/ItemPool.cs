using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    // Start is called before the first frame update

    public List<WeaponData> commonWeapon;
    public List<WeaponData> rareWeapon;
    public List<WeaponData> epicWeapon;
    public List<WeaponData> legendaryWeapon;

    public List<ArmorData> commonArmor;
    public List<ArmorData> rareArmor;
    public List<ArmorData> epicArmor;
    public List<ArmorData> legendaryArmor;

    private int previousWeaponIndex = -1;
    private int previousArmorIndex = -1;

    private int randomIndex = 0;
    public WeaponData GetRandomWeapon(List<WeaponData> weaponList)
    {
        randomIndex = Random.Range(0, weaponList.Count);
        while (randomIndex == previousWeaponIndex)// prevents duplication
        {
            randomIndex = Random.Range(0, weaponList.Count);
            Debug.Log("number:" + randomIndex);
        }
        previousWeaponIndex = randomIndex;
        return weaponList[randomIndex];
    }
    public ArmorData GetRandomArmor(List<ArmorData> armorList)
    {

        randomIndex = Random.Range(0, armorList.Count);
        while (randomIndex == previousArmorIndex)// prevents duplication
        {
            randomIndex = Random.Range(0, armorList.Count);
        }
        previousArmorIndex = randomIndex;

        return armorList[randomIndex];
    }
}
