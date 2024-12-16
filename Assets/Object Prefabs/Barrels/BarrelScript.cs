using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
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
            SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
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
