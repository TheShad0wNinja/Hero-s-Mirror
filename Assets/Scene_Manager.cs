using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerSavedPosition;
    private Stack<string> stack = new();
    public static Scene_Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerSavedPosition = transform.position;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        SavePlayerPosition();
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneAdditive(string sceneName)
    {
        SavePlayerPosition();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        stack.Push(sceneName);
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public void GoToHomebase()
    {
        SavePlayerPosition();
        SceneManager.LoadScene("HomeBase");
    }

    private void SavePlayerPosition()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovementController>()?.gameObject;
        }

        if (player != null)
        {
            playerSavedPosition = player.transform.position;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            stack.Clear();
            stack.Push(scene.name);
        }
        else if (mode == LoadSceneMode.Additive)
        {
            SceneManager.SetActiveScene(scene);
        }

        // player = FindObjectOfType<PlayerMovementController>()?.gameObject;

        // if (player != null)
        // {
        //     player.transform.position = playerSavedPosition;
        // }
    }
}
