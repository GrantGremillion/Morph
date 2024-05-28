using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{

    public Image[] lvlBoxes;
    public Sprite fullLvlBox;
    public Sprite emptyLvlBox;

    public TextMeshProUGUI shopHeaderText;
    public Shop shop;

    public TextMeshProUGUI bowCostText;
    public TextMeshProUGUI healthCostText;
    public Button upgradeBowButton;
    public Button upgradeHealthButton;

    public InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        upgradeBowButton.onClick.AddListener(OnUpgradeBowButtonPressed);
        upgradeBowButton.onClick.AddListener(OnUpgradeHealthButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLvlBoxUI();
        bowCostText.text = "Cost: " + shop.currentBowUpgradeCost;
        healthCostText.text = "Cost: " + shop.currentHealthUpgradeCost;
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

    public void OnUpgradeBowButtonPressed()
    {
        if (shop.canUpgradeBow)
        {
            print("Bow Upgraded");
            inventory.RemoveItems("banana",shop.currentBowUpgradeCost);
            shop.Upgrade();
            
        }
        else return;
    }

     public void OnUpgradeHealthButtonPressed()
    {
        if (shop.canUpgradeHealth)
        {
            print("Health Upgraded");
        }
        else return;
    }

    public void UpdateLvlBoxUI()
    {
        for (int i=0; i < lvlBoxes.Length; i++)
        {

            if(shop.levels > shop.maxLevels)
            {
                shop.levels = shop.maxLevels;
            }

            if (i < shop.levels)
            {
                lvlBoxes[i].sprite = fullLvlBox;
            }
            else
            {
                lvlBoxes[i].sprite = emptyLvlBox;
            }

            if (i < shop.maxLevels)
            {
                lvlBoxes[i].enabled = true;
            }
            else
            {
                lvlBoxes[i].enabled = false;
            }
        }
    }
}
