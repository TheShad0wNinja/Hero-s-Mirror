using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            foreach (Transform child in transform)
            {
                SpriteRenderer spriteRenderer = child.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                Color color = spriteRenderer.color;
                color.a = 0.3f;
                spriteRenderer.color = color;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Transform child in transform)
            {
                SpriteRenderer spriteRenderer = child.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
                }
            }
        }
    }
}
