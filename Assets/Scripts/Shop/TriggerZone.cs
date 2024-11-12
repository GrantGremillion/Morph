using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public bool inRangeOfShop;

    public Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        shop = GetComponentInParent<Shop>();
        inRangeOfShop = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
   
        if (collider.CompareTag("Player"))
        {
            //print("in range");
            inRangeOfShop = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
   
        if (collider.CompareTag("Player"))
        {
            //print("out of range");
            inRangeOfShop = false;
            // If the player leaves the trigger area and is still using the shop, auto-close it
            if (shop.usingShop)
            {
                shop.CloseShop();
            }
        }
    }


}
