using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public bool canUseShop;

    public Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        shop = GetComponentInParent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
   
        if (collider.CompareTag("Player"))
        {
            canUseShop = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
   
        if (collider.CompareTag("Player"))
        {
            canUseShop = false;
            // If the player leaves the trigger area and is still using the shop, auto-close it
            if (shop.usingShop)
            {
                shop.CloseShop();
            }
        }
    }


}
