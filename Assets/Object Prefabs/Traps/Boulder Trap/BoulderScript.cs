using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderScript : MonoBehaviour
{
    private HeroList heroList;
    private List<int> heroes;
    [SerializeField] private float pushForce = 10f;
    UI_Behaviour_Manager inventoryManager;
    [SerializeField] int leastDamage = 1;
    [SerializeField] int MaxDamage = 10;


    private void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(disableMovement(other.gameObject));
            int randomHero = Random.Range(0, inventoryManager.teamAssembleCharacters.Count);
            DamageHero(randomHero);
            
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
