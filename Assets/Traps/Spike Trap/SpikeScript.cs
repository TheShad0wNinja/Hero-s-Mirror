using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float activationTime = 2f;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
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
}
