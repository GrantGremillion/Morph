using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioClip defaultMusic;
    private AudioSource currentMusicSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Start background music
        PlayMusic(defaultMusic, 1f, true);
    }

    public void PlayMusic(AudioClip musicClip, float volume, bool loop)
    {
        // Stop current music if it is playing
        if (currentMusicSource != null)
        {
            currentMusicSource.Stop();
            Destroy(currentMusicSource.gameObject);
        }

        // Spawn and play the new music
        currentMusicSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        currentMusicSource.clip = musicClip;
        currentMusicSource.volume = volume;
        currentMusicSource.loop = loop;
        currentMusicSource.Play();
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
}
