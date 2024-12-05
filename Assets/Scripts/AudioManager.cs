using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public Sound[] musicSounds, sfxSounds;
    public Sound[] mainMenu, tutorialLevel, mainBase, offenseLevel, defenseLevel;
    public AudioSource musicSource, sfxSource;
    public UnityEngine.UI.Slider musicSlider, sfxSlider;
    
    [SerializeField]
    public float musicVolume, sfxVolume;

    private string currentMusic;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlayMainMenuMusic()
    {
        Sound s = mainMenu[UnityEngine.Random.Range(0, mainMenu.Length)];
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlayTutorialMusic()
    {
        Sound s = tutorialLevel[UnityEngine.Random.Range(0, tutorialLevel.Length)];
        musicSource.clip = s.clip;
        musicSource.Play();
    }
    public void PlayMainBaseMusic()
    {
        Sound s = mainBase[UnityEngine.Random.Range(0, mainBase.Length)];
        musicSource.clip = s.clip;
        musicSource.Play();
    }
    public void PlayOffenseLevelMusic()
    {
        Sound s = offenseLevel[UnityEngine.Random.Range(0, offenseLevel.Length)];
        musicSource.clip = s.clip;
        musicSource.Play();
    }
    public void PlayDefenseLevelMusic()
    {
        Sound s = defenseLevel[UnityEngine.Random.Range(0, defenseLevel.Length)];
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }

    public void SetMusicVolume()
    {
        musicSource.volume = musicSlider.value;
    }
    
    public void SetSFXVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }
}