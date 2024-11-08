using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused }
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Playing;
        // Notify other scripts if needed
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
