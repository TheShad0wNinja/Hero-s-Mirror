using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float activationTime = 2f;
    private bool trigger; 
    private Animator anim;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!trigger)
            {
                StartCoroutine(ActivateTrap());
            }
        }
    }

    private IEnumerator ActivateTrap()
    {
        while(true)
        {
        trigger = true;
        anim.SetBool("activated", true);
        GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(activationTime);
        GetComponent<Collider2D>().enabled = false;
        anim.SetBool("activated", false);
        trigger = false;
        yield return new WaitForSeconds(activationTime);
        }
    }
}
