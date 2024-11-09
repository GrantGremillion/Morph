using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused }
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (SceneManager.GetActiveScene().name != "Main" && CurrentGameState != GameState.Paused)
        {
            CurrentGameState = GameState.Playing;
        }
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Playing;
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
    }

    public void GoToMainMenu()
    {
        CurrentGameState = GameState.MainMenu;
    }
}
