using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public int itemCount;
    public TextMeshProUGUI itemText;

    // Start is called before the first frame update
    void Start()
    {   
        itemCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemText();
    }

    // Update the text value of the TextMeshPro component
    void UpdateItemText()
    {
        if (itemText != null)
        {
            itemText.text = "Items: " + itemCount.ToString();
        }
    }
}
