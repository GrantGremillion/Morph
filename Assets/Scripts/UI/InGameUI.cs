using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{

    public TextMeshProUGUI itemText;
    public InventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     // Update the text value of the TextMeshPro component
    public void UpdateItemText()
    {
        if (itemText != null)
        {
            itemText.text = "Items: " + inventoryManager.itemCount.ToString();
        }
    }
}
