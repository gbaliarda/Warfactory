using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioSource))]
public class BulletSoundController : MonoBehaviour, ISoundable
{
    public Sound[] bullets;
    private AudioSource _audioSource;
    private int index;

    public void PlaySound(Sound sound)
    {
        _audioSource.PlayOneShot(sound.clip);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (index >= bullets.Length) index = 0;
        PlaySound(bullets[index++]);
    }

}
