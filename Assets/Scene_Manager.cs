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
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
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
    }

    public void GoToPreviousScene()
    {
        string currScene = stack.Pop();
        SceneManager.UnloadSceneAsync(currScene);
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
        Debug.Log("Loaded: " + scene.name);
        if (mode == LoadSceneMode.Single)
        {
            stack.Clear();
        }
        else if (mode == LoadSceneMode.Additive)
        {
            SceneManager.SetActiveScene(scene);
        }
        stack.Push(scene.name);

        // player = FindObjectOfType<PlayerMovementController>()?.gameObject;

        // if (player != null)
        // {
        //     player.transform.position = playerSavedPosition;
        // }
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("UNLOADED: " + current.name);
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(stack.Peek()));
    }
}
