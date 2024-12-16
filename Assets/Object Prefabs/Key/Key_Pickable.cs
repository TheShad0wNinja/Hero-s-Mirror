using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key_Pickable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject textBubblePrefab;
    private GameObject textBubbleInstance;
    PuzzleDoor puzzleDoor;
    DoorScript doorScript;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textBubbleInstance == null) 
            {
                textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
            }
            textBubbleInstance.GetComponentInChildren<TextMeshPro>().text = "Key Acquired";
            textBubbleInstance.transform.SetParent(transform);
            if(FindAnyObjectByType<DoorScript>() == null){
                FindObjectOfType<PuzzleDoor>().UnlockDoorKey();
            }
            else{
                FindObjectOfType<DoorScript>().UnlockDoorKey();
            }
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(textBubbleInstance);
        }
    }
}
