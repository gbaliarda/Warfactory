using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioSource))]
public class StepSoundController : MonoBehaviour, ISoundable
{
    public Sound[] grassWalk, groundWalk;
    private AudioSource _audioSource;
    private int index;

    private Actor _actor;
    private float stepTimer;
    private float stepDelay;

    public void PlaySound(Sound sound)
    {
        _audioSource.PlayOneShot(sound.clip);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _actor = GetComponent<Actor>();

        stepDelay = 2f / (_actor.Stats.MovementSpeed);
    }

    void Update()
    {
        if (_actor != null && _actor.IsMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0)
            {
                int type = TileManager.Instance.TileType(_actor.transform.position);

                switch (type)
                {
                    case 2:
                        if (index >= groundWalk.Length) index = 0;
                        PlaySound(groundWalk[index++]);
                        break;
                    case 1:
                        if (index >= grassWalk.Length) index = 0;
                        PlaySound(grassWalk[index++]);
                        break;
                }

                stepTimer = stepDelay;
            }
        }   
    }
}
