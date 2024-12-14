using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float activationTime = 2f;
    private Animator anim;
    UI_Behaviour_Manager inventoryManager;
    [SerializeField] int leastDamage = 1;
    [SerializeField] int MaxDamage = 10;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        StartCoroutine(ActivateTrap());
    }

    private IEnumerator ActivateTrap()
    {
        while(true)
        {
        anim.SetBool("activated", true);
        GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(activationTime);
        GetComponent<Collider2D>().enabled = false;
        anim.SetBool("activated", false);
        yield return new WaitForSeconds(activationTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int randomHero = Random.Range(0, inventoryManager.teamAssembleCharacters.Count);
            DamageHero(randomHero);
        }
    }

    private void DamageHero(int hero)
    {
        bool canBeDamaged = true;
        int count = 0;
        int index = hero;
        while (canBeDamaged)
        {
            int damage = Random.Range(leastDamage, MaxDamage + 1);
            if (inventoryManager.teamAssembleCharacters[index].currentHealth > 0)
            {
                inventoryManager.teamAssembleCharacters[index].currentHealth -= damage;
                canBeDamaged = false;
                FindObjectOfType<Level_UI_Manager>().UpdateUI();
            }
            else 
            {
                count++;
                index = (index +1) %3;
            }
            if (count == 2) 
            {
                canBeDamaged = false;
            }
        }
    }
}
