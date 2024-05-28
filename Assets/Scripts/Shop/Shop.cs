using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public bool usingShop = false;

    public ShopUI shopUI;

    // Start is called before the first frame update
    void Start()
    {
        shopUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenShop()
    {
        usingShop = true;
        shopUI.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        usingShop = false;
        shopUI.gameObject.SetActive(false);
    }

}
