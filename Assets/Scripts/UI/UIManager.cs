using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{

    public MainMenuUI mainMenuUI;
    public ShopUI shopUI;
    public InGameUI inGameUI;

    public PlayerController player;

    

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        mainMenuUI.gameObject.SetActive(true);
        shopUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.canPlay) inGameUI.gameObject.SetActive(true);
    }

}
