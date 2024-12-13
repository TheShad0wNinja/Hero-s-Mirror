using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingTrap : MonoBehaviour
{
    [SerializeField] private Transform teleportLocation;
    [SerializeField] private float collapseDelay = 2f;
    private bool isTriggered = false;
    private GameObject playerOnFloor = null;
    SpriteRenderer spriteRenderer;
    Color color;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            playerOnFloor = other.gameObject;
            StartCoroutine(CollapseFloor(other.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerOnFloor)
        {
            isTriggered = false;
            playerOnFloor = null;
            StopAllCoroutines();
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }

    private IEnumerator CollapseFloor(GameObject player)
    {
        if (spriteRenderer != null)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime / collapseDelay)
            {
                color.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = color;
                yield return null;
            }
        }

        if (player != null && teleportLocation != null)
        {
            player.transform.position = teleportLocation.position;
        }
        
        isTriggered = false;
    }
}
