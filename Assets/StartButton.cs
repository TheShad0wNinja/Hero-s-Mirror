using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    Scene_Manager sceneManager;


    void Start()
    {
        sceneManager = FindObjectOfType<Scene_Manager>();
    }

    public void StartGame()
    {
        sceneManager.ChangeScene("Level1");
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
