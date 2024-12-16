using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudio : MonoBehaviour
{
    Audio_Manager audioManager;
    AudioClip audioClip;

    void Start()
    {
        audioManager = FindObjectOfType<Audio_Manager>();
        audioClip = Resources.Load<AudioClip>("mainMenuAudio");
        Debug.Log(audioClip);

        audioManager.PlayMusic(audioClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
