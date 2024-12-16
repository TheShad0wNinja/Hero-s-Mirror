using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftingPuzzelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerPuzzelController Player;
    public bool WithinPlayerRadius = false;
    public bool puzzelCompleted = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreatePuzzelItem()
    {
Debug.Log("teehee");
        if (Player.inventory.Count < 3 && WithinPlayerRadius)
        {
            Debug.Log("You still need to collect all the items");
        }
        else if (Player.inventory.Count == 3 && WithinPlayerRadius)
        {
            Debug.Log("All items were picked up");
            SceneManager.LoadScene(11); // Loads the scene at index 1 in Build Settings
            puzzelCompleted = true; // opens the door
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