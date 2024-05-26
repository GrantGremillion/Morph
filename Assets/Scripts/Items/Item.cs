using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{

    public InventoryManager inventory;

    public String type;

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
        inventory = FindObjectOfType<InventoryManager>();

        if (collider.CompareTag("Player"))
        {
            inventory.AddItem(this.type);
            Destroy(gameObject);
        }
    }
}
