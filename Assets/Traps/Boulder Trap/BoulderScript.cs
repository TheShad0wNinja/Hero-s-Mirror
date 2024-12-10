using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderScript : MonoBehaviour
{
    private HeroList heroList;
    private List<int> heroes;


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
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int randomHero = heroes[Random.Range(0, heroes.Count)];
            damageHero(randomHero);
        }
        Destroy(gameObject);
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
