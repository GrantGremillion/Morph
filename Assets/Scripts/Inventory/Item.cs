using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{

    public InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        
        if (collider.CompareTag("Player"))
        {
            print("collision");
            inventory.itemCount ++;
            Destroy(gameObject);
            print(inventory.itemCount);
        }
    }
}
