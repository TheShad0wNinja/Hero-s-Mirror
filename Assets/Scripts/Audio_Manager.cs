using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    
    [SerializeField] private int maxAudioSources = 10;
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentAudioSourceIndex = 0;

    private AudioSource musicSource;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // If there is already an instance of this class, destroy this one
            Destroy(this.gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        for(int i = 0; i < maxAudioSources; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(newSource);
        }
    }

    public void PlayMusic(AudioClip musicClip, float volume = 0.1f)
    {
        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip, float volume = 0.1f, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        // Set the audio source to the current index, set the clip and volume, and play it
        sfxSources[currentAudioSourceIndex].pitch = Random.Range(minPitch, maxPitch);
        sfxSources[currentAudioSourceIndex].clip = sfxClip;
        sfxSources[currentAudioSourceIndex].volume = volume;
        sfxSources[currentAudioSourceIndex].Play();

        // Increment the index and loop back to 0 if it exceeds the max number of audio sources
        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % maxAudioSources;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        foreach(AudioSource source in sfxSources)
        {
            source.Stop();
        }
    }

    public void StopAllAudio()
    {
        StopMusic();
        StopSFX();
    }
}
