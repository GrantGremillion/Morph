using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioClip music;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Start background music
        PlaySoundFXClip(music,transform,1f,true);
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool loop)
    {
        // spawn audio gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        if (loop) audioSource.loop = true;
        else audioSource.loop = false;
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        if (!loop) Destroy(audioSource.gameObject, clipLength);

    }
}
