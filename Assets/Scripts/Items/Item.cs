using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{

    public InventoryManager inventory;

    public String type;
    [SerializeField] private AudioClip pickupItemSound;

    void OnTriggerEnter2D (Collider2D collider)
    {
        inventory = FindObjectOfType<InventoryManager>();

        if (collider.CompareTag("Player"))
        {
            SoundFXManager.instance.PlaySoundFXClip(pickupItemSound, transform, 1f, false);
            inventory.AddItem(this.type);
            Destroy(gameObject);
        }
    }

}
