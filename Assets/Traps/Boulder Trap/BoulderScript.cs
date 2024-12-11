using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderScript : MonoBehaviour
{
    private HeroList heroList;
    private List<int> heroes;
    [SerializeField] private float pushForce = 10f;


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
            StartCoroutine(disableMovement(other.gameObject));
            int randomHero = heroes[Random.Range(0, heroes.Count)];
            damageHero(randomHero);
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator disableMovement(GameObject player)
    {
        player.GetComponent<PlayerMovementController>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerMovementController>().enabled = true;
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
