using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{
    public TextMeshProUGUI shopHeaderText;
    public Shop shop;

    public TextMeshProUGUI bowCostText;
    public TextMeshProUGUI healthCostText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        if (shop.canUpgradeBow)
        {
            bowCostText.color = Color.white;
        }
        else bowCostText.color = Color.red;

        if (shop.canUpgradeHealth)
        {
            healthCostText.color = Color.white;
        }
        else healthCostText.color = Color.red;
    }
}
