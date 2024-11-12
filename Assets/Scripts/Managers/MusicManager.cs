using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip MainMenuMusic;
    [SerializeField] private AudioClip HubMusic;
    [SerializeField] private AudioClip Level1Music;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing && SceneManager.GetActiveScene().name == "Main") PlaySong(HubMusic);
        else if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing && SceneManager.GetActiveScene().name == "Lvl1") PlaySong(Level1Music);
    }

    public void PlaySong(AudioClip song)
    {
        // Check if the song is already playing; if so, do nothing.
    if (audioSource.clip == song && audioSource.isPlaying)
        return;

    // Stop the current song if there's one playing
    audioSource.Stop();
        audioSource.clip = song;
        audioSource.Play();
    }
}
