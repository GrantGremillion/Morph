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

    public PlayerController player;

    internal void PauseMenuUI(bool v)
    {
        if (v) pausedMenuUI.gameObject.SetActive(true);
        else pausedMenuUI.gameObject.SetActive(false);
    }



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
        

        shopUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
        {
            inGameUI.gameObject.SetActive(true);
        }
        
    }

}
