using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{

    public MainMenuUI mainMenuUI;
    public ShopUI shopUI;
    public InGameUI inGameUI;
    public PausedMenuUI pausedMenuUI;
    public SettingsMenuUI settingsMenuUI;

    public PlayerController player;

    private bool paused;

    

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        if (SceneManager.GetActiveScene().name == "Main")
        {
            mainMenuUI.gameObject.SetActive(true);
        }
        else
        {
            mainMenuUI.gameObject.SetActive(false);
            inGameUI.gameObject.SetActive(true);
        } 
        
        pausedMenuUI.gameObject.SetActive(false);
        settingsMenuUI.gameObject.SetActive(false);
        shopUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);

        paused = false;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
        {
            inGameUI.gameObject.SetActive(true);
        }

        // Open/Close settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing || 
            GameManager.Instance.CurrentGameState == GameManager.GameState.Paused) PauseMenuUI();
        }
        
    }

    public void PauseMenuUI()
    {
        if (!paused)  
        {
            //print ("Pause"); 
            GameManager.Instance.PauseGame();
            paused = true;
            pausedMenuUI.gameObject.SetActive(true);
        }
        else 
        {
            //print ("Unpause");
            GameManager.Instance.StartGame();
            paused = false;
            pausedMenuUI.gameObject.SetActive(false);
            settingsMenuUI.gameObject.SetActive(false);
        }   
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ViewSettings()
    {
        pausedMenuUI.gameObject.SetActive(false);
        settingsMenuUI.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        settingsMenuUI.gameObject.SetActive(false);
        pausedMenuUI.gameObject.SetActive(true);
    }

}
