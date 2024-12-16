using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Windows;
public class CraftingPuzzelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerPuzzelController Player;
    public bool WithinPlayerRadius = false;
    public bool puzzelCompleted = false;
    [SerializeField] private GameObject textBubblePrefab;
    private GameObject textBubbleInstance;
    public DoorScript puzzelCompleteddoor;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D Other)
    {

        if (Other.gameObject.tag == "Player" || Other.gameObject.tag == "DetectionRadius")
        {

            if (Player.inventory.Count < 3)
            {
                InstantiateBubble("find items to start puzzel");

                Debug.Log("You still need to collect all the items");
            }
            else if (Player.inventory.Count == 3)
            {
                

                Debug.Log("All items were picked up");
                SceneManager.LoadScene("CraftingTableUI");



            }
        }


    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(textBubbleInstance);
        }

    }
    void InstantiateBubble(string input)
    {
        textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
        textBubbleInstance.GetComponentInChildren<TextMeshPro>().text = input;
        textBubbleInstance.transform.SetParent(transform);
    }

}