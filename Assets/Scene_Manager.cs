using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private Audio_Manager audio_Manager;
    public AudioClip backgroundMusic;

    private GameObject prevPlayer;
    private Vector3 prevPlayerPosition;
    private bool hasPrevPosition = false;
    private Light2D[] prevLights;
    private Stack<string> stack = new();
    public static Scene_Manager Instance { get; private set; }

    void Start()
    {
        audio_Manager = FindObjectOfType<Audio_Manager>();

    }

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

        // Load the background music, ensuring the file is in Resources/forest.wav (without extension in the path)

        switch (scene.name)
        {
            case "HomeBase_Old_Combat":
                backgroundMusic = Resources.Load<AudioClip>("HomeBase_Old_Combat");
                audio_Manager.PlayMusic(backgroundMusic);
                break;
            case "Forest":
                backgroundMusic = Resources.Load<AudioClip>("forest");
                audio_Manager.PlayMusic(backgroundMusic,10);
                break;
            case "HomeBase":
                backgroundMusic = Resources.Load<AudioClip>("HomeBase");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Barracks":
                backgroundMusic = Resources.Load<AudioClip>("Barracks");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Barracks 1":
                backgroundMusic = Resources.Load<AudioClip>("Barracks");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Barracks 2":
                backgroundMusic = Resources.Load<AudioClip>("Barracks");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Dungeon 2":
                backgroundMusic = Resources.Load<AudioClip>("Dungeon");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Dungeon 3":
                backgroundMusic = Resources.Load<AudioClip>("Dungeon");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Duke 1":
                backgroundMusic = Resources.Load<AudioClip>("HomeBase");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Duke 2":
                backgroundMusic = Resources.Load<AudioClip>("HomeBase");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
            case "Duke 3":
                backgroundMusic = Resources.Load<AudioClip>("HomeBase");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
                  case "Duke's Manory L3":
                backgroundMusic = Resources.Load<AudioClip>("Resources/Dukes Manor");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;
                  case "Throne Room":
                backgroundMusic = Resources.Load<AudioClip>("Throne Room");
                audio_Manager.PlayMusic(backgroundMusic, 10);
                break;




            default:
                break;
        }

        if (mode == LoadSceneMode.Single)


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
