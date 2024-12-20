using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private GameObject prevPlayerInstance;
    private Light2D[] prevLights;

    private Vector3 prevPlayerPosition;
    private bool hasPrevPosition = false;
    private bool shouldRestorePosition = false;

    private Stack<string> currentLoadedScenes = new();
    public static Scene_Manager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded: " + scene.name);

        if (mode == LoadSceneMode.Single)
            currentLoadedScenes.Clear();
        else if (mode == LoadSceneMode.Additive)
            SceneManager.SetActiveScene(scene);
        currentLoadedScenes.Push(scene.name);

        if (shouldRestorePosition)
            ReturnPlayerPosition();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Unloaded: " + scene.name);
    }

    public void ChangeScene(string sceneName)
    {
        shouldRestorePosition = false;
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneWithSave(string sceneName)
    {
        SavePlayerPosition();
        SceneManager.LoadScene(sceneName);
    }

    private void SavePlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            prevPlayerPosition = player.transform.position;
            hasPrevPosition = true;
        }
    }

    private void ReturnPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        if (hasPrevPosition && player != null)
        {
            player.transform.position = prevPlayerPosition;
            hasPrevPosition = false;
            shouldRestorePosition = false;
        }
    }

    public void GoToHomebase()
    {
        SceneManager.LoadScene("HomeBase");
        shouldRestorePosition = true;
    }

    public void LoadAdditiveScene(string sceneName)
    {
        SavePlayerInstance();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }


    public void LoadAdditiveSceneRemoveLight(string sceneName)
    {
        SavePreviousLight();
        SavePlayerInstance();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    private void SavePlayerInstance()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            prevPlayerInstance = player;
            player.SetActive(false);
        }
    }

    private void SavePreviousLight()
    {
        var lights = FindObjectsOfType<Light2D>();
        prevLights = lights;
        foreach(var light in lights)
            light.gameObject.SetActive(false);
    }

    public void UnloadAdditiveScene()
    {
        string currScene = currentLoadedScenes.Pop();
        SceneManager.UnloadSceneAsync(currScene);
        ReturnPlayerInstance();
        ReturnPreviousLIght();
    }

    private void ReturnPreviousLIght()
    {
        foreach(var light in prevLights)
            light.gameObject.SetActive(true);
        prevLights = null;
    }

    private void ReturnPlayerInstance()
    {
        if (prevPlayerInstance != null)
            prevPlayerInstance.SetActive(true);
        prevPlayerInstance = null;
    }

}
