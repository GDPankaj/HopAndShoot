using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] musicSounds, sfxSounds;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    public static AudioManager instance;
    bool musicPlaying = false;
    bool ismusicMuted = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    private void Start()
    {
        
        if (musicAudioSource != null && !musicAudioSource.gameObject.activeInHierarchy)
        {
            musicAudioSource.gameObject.SetActive(true);
        }
        if (sfxAudioSource != null && !sfxAudioSource.gameObject.activeInHierarchy)
        {
            sfxAudioSource.gameObject.SetActive(true);
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!musicPlaying)
        {
            PlayMusic("BackgroundMusic");
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.GetName() == name);

        if(s == null)
        {
            Debug.Log($"No sound named {name}");
        }
        else
        {
            musicAudioSource.clip = s.GetClip();
            musicAudioSource.Play();
            musicAudioSource.loop = true;
            musicPlaying = true;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.GetName() == name);

        if (s == null)
        {
            Debug.Log($"No sound named {name}");
        }
        else
        {
            sfxAudioSource.PlayOneShot(s.GetClip());
            Debug.Log($"{name} is playing");
        }
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
        musicPlaying = false;
    }

    public void MuteMusic()
    {
        if(!musicAudioSource.mute)
        {
            musicAudioSource.mute = true;
        }
        else
        {
            musicAudioSource.mute = false;
        }

        ismusicMuted = musicAudioSource.mute;
            
    }

    public bool IsMusicMuted()
    {
        return ismusicMuted;
    }
}
