using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sfx;
    public Sound[] music;

    private void Awake()
    {
        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = System.Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopMusic(string name)
    {
        Sound s = System.Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
    
    public void SetMusicVolume(float volume)
    {
        foreach (Sound s in music)
        {
            s.source.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach (Sound s in sfx)
        {
            s.source.volume = volume;
        }
    }
    
    // Fade out music with a given name over a given time
    // to smoothly transition between music tracks
    public IEnumerator FadeOut(string name, float fadeTime)
    {
        Sound s = System.Array.Find(music, sound => sound.name == name);
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        s.source.Stop();
    }
}