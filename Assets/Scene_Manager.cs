using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private GameObject prevPlayer;
    private Vector3 prevPlayerPosition;
    private bool hasPrevPosition = false;
    private Light2D[] prevLights;
    private Stack<string> stack = new();
    public static Scene_Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        SavePlayerInstance();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        stack.Push(sceneName);
    }

    public void ChangeSceneAdditiveRemoveLight(string sceneName)
    {
        SavePreviousLight();
        SavePlayerInstance();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        stack.Push(sceneName);
    }


    public void GoToPreviousSceneAdditive()
    {
        string currScene = stack.Pop();
        SceneManager.UnloadSceneAsync(currScene);
        ReturnPlayerInstance();
        ReturnLights();
    }

    public void GoToHomebase()
    {
        SceneManager.LoadScene("HomeBase");
    }

    public void GoToHomebaseOrigin()
    {
        hasPrevPosition = false;
        SceneManager.LoadScene("HomeBase");
    }

    private void ReturnLights()
    {
        if (prevLights != null)
        {
            foreach (var light in prevLights)
            {
                if (light.lightType == Light2D.LightType.Global)
                    light.gameObject.SetActive(true);
            }

            prevLights = null;
        }
    }

    private void SavePreviousLight()
    {
        var lights = FindObjectsOfType<Light2D>();
        foreach (var light in lights)
        {
            if (light.lightType == Light2D.LightType.Global)
                light.gameObject.SetActive(false);
        }
    }

    private void ReturnPlayerInstance()
    {
        if (prevPlayer != null)
        {
            prevPlayer.SetActive(true);
            prevPlayer = null;
        }
    }

    private void SavePlayerInstance()
    {
        prevPlayer = FindObjectOfType<PlayerMovementController>()?.gameObject;
        prevPlayer?.SetActive(false);
    }

    private void ReturnPlayerPosition()
    {
        var player = FindObjectOfType<PlayerMovementController>();
        if (hasPrevPosition && player != null)
        {
            player.transform.position = prevPlayerPosition;
            hasPrevPosition = false;
        }
    }

    private void SavePlayerPosition()
    {
        var player = FindObjectOfType<PlayerMovementController>();
        if (player != null)
        {
            hasPrevPosition = true;
            prevPlayerPosition = player.transform.position;
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded: " + scene.name);
        if (mode == LoadSceneMode.Single)
        {
            stack.Clear();
            ReturnPlayerPosition();
        }
        else if (mode == LoadSceneMode.Additive)
        {
            SceneManager.SetActiveScene(scene);
        }
        stack.Push(scene.name);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Unloaded: " + scene.name);
    }
}
