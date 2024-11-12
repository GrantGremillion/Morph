using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    [SerializeField] private AudioClip walk;
    public AudioSource walkAudioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        walkAudioSource = Instantiate(soundFXObject, new Vector3 (0, 0, 0), Quaternion.identity);
        walkAudioSource.clip = walk;
        walkAudioSource.loop = true;
    }


    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool loop)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        if (!loop) Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayFootstepsSoundFXClip()
    {
        walkAudioSource.Play();
    }

    public void StopFootstepsSoundFXClip()
    {
        walkAudioSource.Pause();
    }
}
