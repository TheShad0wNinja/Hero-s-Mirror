using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Start is called before the first frame update
    public FireBlockage newFire;
    public bool FireState = true;
    public bool WithinPlayerRadius = false;
    private Animator animator;

    void Start()
    {

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Interact()
    {
        if (WithinPlayerRadius)
        {
            animator.SetBool("TurnedOn",FireState );
            FireState = newFire.FireSwitch(FireState);
        }
    }
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player" || Other.gameObject.tag == "DetectionRadius")
        {
            WithinPlayerRadius = true;
        }
        else
        {
            Debug.Log("You are too far away");
            WithinPlayerRadius = false;


        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DetectionRadius") || other.CompareTag("Player"))
        {
            WithinPlayerRadius = false;

        }

    }
}
