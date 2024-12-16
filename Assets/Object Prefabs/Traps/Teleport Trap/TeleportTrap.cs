using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrap : MonoBehaviour
{
    [SerializeField] private Transform teleportLocation;
    private bool isTriggered;
    private GameObject playerOnFloor = null;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float startingAlpha = 0f;
    [SerializeField] private float teleportTime = 0.7f;
    Audio_Manager audioManager;
    AudioClip audioClip;



private void Start()
{
    audioManager = FindObjectOfType<Audio_Manager>();
    audioClip = Resources.Load<AudioClip>("teleportSound");
    spriteRenderer = GetComponent<SpriteRenderer>();
    SetSpriteAlpha(startingAlpha);
}

private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            playerOnFloor = other.gameObject;
            StartCoroutine(TeleportPlayer(other.gameObject));
            StartCoroutine(FadeInSprite());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerOnFloor)
        {
            isTriggered = false;
            playerOnFloor = null;
            StopAllCoroutines();
            SetSpriteAlpha(startingAlpha);
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        yield return new WaitForSeconds(teleportTime);

        if (player != null && teleportLocation != null)
        {
            player.transform.position = teleportLocation.position;
            audioManager.PlaySFX(audioClip);
        }
        isTriggered = false;
    }

    private IEnumerator FadeInSprite()
{
    float alpha = spriteRenderer.color.a;
    while (alpha < 1f)
    {
        alpha += Time.deltaTime;
        SetSpriteAlpha(alpha);
        yield return null;
    }
    SetSpriteAlpha(1f);
}

    private void SetSpriteAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}

