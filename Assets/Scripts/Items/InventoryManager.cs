using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<string> items = new List<string>();

    public GameObject banana; 
    public GameObject blueberry; 
    public GameObject cherry; 

    public void AddItem(string item)
    {
       if (item != null)
        {
            items.Add(item);
            Debug.Log("Item added: " + item + ". Total items: " + items.Count);
            
        }
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (string item in items)
        {
            if (item == itemName)
            {
                count++;
            }
        }
        return count;
    }


    public void DropItems()
    {

        if (items.Count == 0) return;

        string randomItem = RandomElementSelector.GetRandomElement(items);
        print("Randomly selected item: " + randomItem);

        if (randomItem == "banana")
        {
            Instantiate(banana);
        }
        else if (randomItem == "blueberry")
        {
            Instantiate(blueberry);
        }
        else
        {
              Instantiate(cherry);
        }
        items.Remove(randomItem);
        
    }

}
