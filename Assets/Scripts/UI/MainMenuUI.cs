using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MainMenuUI : MonoBehaviour
{

    public PlayerController player;
    public Button StartButton;
    public Button OptionsButton;
    public Button QuitButton;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        StartButton.onClick.AddListener(OnStartButtonPressed);
        OptionsButton.onClick.AddListener(OnOptionsButtonPressed);
        QuitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPressed()
    {
        gameObject.SetActive(false);
        player.canPlay = true;
    }
    public void OnOptionsButtonPressed()
    {
        
    }
    public void OnQuitButtonPressed()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
