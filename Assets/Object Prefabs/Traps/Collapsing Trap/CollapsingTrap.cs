using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollapsingTrap : MonoBehaviour
{
    [SerializeField] private float collapseDelay = 2f;
    private GameObject playerOnFloor = null;
    SpriteRenderer spriteRenderer;
    Color color;
    [SerializeField] string teleportScene;
    public bool returnTeleporter;
    bool didTeleport = false;
    Audio_Manager audioManager;
    AudioClip audioClip;

    void Awake()
    {
        audioManager = FindObjectOfType<Audio_Manager>();
        audioClip = Resources.Load<AudioClip>("CollapsingTrap");
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !didTeleport)
        {
            playerOnFloor = other.gameObject;
            StartCoroutine(CollapseFloor(other.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerOnFloor)
        {
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

        if(returnTeleporter == true){
            audioManager.PlaySFX(audioClip);
            Scene_Manager.Instance.GoToPreviousSceneAdditive();
        }
        else{
            audioManager.PlaySFX(audioClip);
            Scene_Manager.Instance.ChangeSceneAdditive(teleportScene);
        }
        didTeleport = true;
    }

// private IEnumerator CollapseFloor(GameObject player)
// {
//     Debug.Log("Starting CollapseFloor Coroutine");

//     // Fade out the floor's sprite
//     if (spriteRenderer != null)
//     {
//         for (float t = 0; t < 1f; t += Time.deltaTime / collapseDelay)
//         {
//             color.a = Mathf.Lerp(1f, 0f, t);
//             spriteRenderer.color = color;
//             yield return null; // Ensures coroutine execution
//         }
//     }

//     Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);
//     Debug.Log("Next Scene: " + teleportScene);

//     // Deactivate player before loading the new scene
//     Debug.Log("Deactivating player...");
//     player.GetComponent<PlayerMovementController>().enabled = false;

//     // Load the next scene
//     Debug.Log($"Loading scene: {teleportScene}");
//     AsyncOperation loadOperation = SceneManager.LoadSceneAsync(teleportScene, LoadSceneMode.Additive);

//     while (!loadOperation.isDone)
//     {
//         Debug.Log($"Scene Loading Progress: {loadOperation.progress}");
//         yield return null; // Waits until the scene is fully loaded
//     }

//     Debug.Log($"Scene '{teleportScene}' loaded.");

//     // Verify the scene is loaded and valid
//     Scene loadedScene = SceneManager.GetSceneByName(teleportScene);
//     Debug.Log($"Scene '{teleportScene}' IsValid: {loadedScene.IsValid()}, IsLoaded: {loadedScene.isLoaded}");

//     if (loadedScene.IsValid() && loadedScene.isLoaded)
//     {
//         Debug.Log($"Setting active scene to: {teleportScene}");
//         SceneManager.SetActiveScene(loadedScene);
//         Debug.Log("Active Scene Set: " + SceneManager.GetActiveScene().name);
//     }
//     else
//     {
//         Debug.LogError($"Failed to set active scene: {teleportScene}");
//     }

//     isTriggered = false;
// }




}
