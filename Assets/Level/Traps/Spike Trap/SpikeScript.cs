using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float activationTime = 2f;
    private Animator anim;
    private HeroList heroList;
    private List<int> heroes;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        heroList = FindObjectOfType<HeroList>();
        if (heroList != null)
        {
            heroes = heroList.HeroLists;
            foreach (int hero in heroes)
            {
                Debug.Log("Hero ID: " + hero);
            }
        }
        else
        {
            Debug.LogError("HeroList not found!");
        }
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
            int randomHero = heroes[Random.Range(0, heroes.Count)];
            damageHero(randomHero);
        }
    }

    private void damageHero(int hero)
    {
        int damage = Random.Range(1, 11);
        hero -= damage;
        Debug.Log("Hero took " + damage + " damage. Remaining health: " + hero);
        int index = heroes.IndexOf(hero + damage);
        if (index != -1)
        {
            heroes[index] = hero;
        }
    }
}
