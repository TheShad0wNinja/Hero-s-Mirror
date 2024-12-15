using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key_Pickable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject textBubblePrefab;
    [SerializeField] private GameObject door;
    private GameObject textBubbleInstance;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
            textBubbleInstance.GetComponentInChildren<TextMeshPro>().text = "Key Acquired";
            textBubbleInstance.transform.SetParent(transform);
            door.GetComponent<DoorScript>().UnlockDoorKey();
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