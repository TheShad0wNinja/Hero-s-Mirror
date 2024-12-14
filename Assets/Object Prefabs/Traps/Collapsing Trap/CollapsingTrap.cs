using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollapsingTrap : MonoBehaviour
{
    [SerializeField] private float collapseDelay = 2f;
    private bool isTriggered = false;
    private GameObject playerOnFloor = null;
    SpriteRenderer spriteRenderer;
    Color color;
    [SerializeField] string nextSceneName;


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

        if(SceneManager.GetActiveScene().name != nextSceneName)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextSceneName));
            Time.timeScale = 1f;
        }
        else if(SceneManager.GetActiveScene().name == nextSceneName)
        {
            SceneManager.UnloadSceneAsync(nextSceneName);
            Time.timeScale = 1f;
        }
        
        isTriggered = false;
    }
}
