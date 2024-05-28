using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InGameUI : MonoBehaviour
{

    // Item UI variables
    public TextMeshProUGUI itemText;
    public InventoryManager inventoryManager;


    // Health UI variables
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


    public PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI(); 
        UpdateItemText();
    }

     // Update the text value of the TextMeshPro component
    public void UpdateItemText()
    {
        if (itemText != null)
        {
            itemText.text = inventoryManager.GetItemCount("banana").ToString() 
            + "\n" + inventoryManager.GetItemCount("blueberry").ToString()
            + "\n" + inventoryManager.GetItemCount("cherry").ToString();
        }
    }

    public void UpdateHealthUI()
    {
        for (int i=0; i < hearts.Length; i++)
        {

            if(player.health > player.numOfHearts)
            {
                player.health = player.numOfHearts;
            }

            if (i < player.health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < player.numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
