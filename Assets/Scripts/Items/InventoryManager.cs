using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log("Item added: " + item.GetType().Name + ". Total items: " + items.Count);
    }

    public int GetItemCount<T>() where T : Item
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item is T)
            {
                count++;
            }
        }
        return count;
    }

}
