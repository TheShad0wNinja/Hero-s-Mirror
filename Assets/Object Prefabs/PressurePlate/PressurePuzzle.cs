using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePuzzle : MonoBehaviour
{
    [SerializeField] Sprite spriteAfter;
    SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject door;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = spriteAfter;
            door.GetComponent<DoorScript>().UnlockDoorPuzzle();
        }
    }
}
