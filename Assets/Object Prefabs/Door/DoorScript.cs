using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    PlayerMovementController player;
    [SerializeField] Sprite openDoor;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        player = FindObjectOfType<PlayerMovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(player.runeSolved == true)
        {
            spriteRenderer.sprite = openDoor;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
