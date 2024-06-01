using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{

    public PlayerController player;
    public Image[] lvlBoxes;
    public Sprite fullLvlBox;
    public Sprite emptyLvlBox;

    public TextMeshProUGUI shopHeaderText;
    public Shop shop;

    public TextMeshProUGUI bowCostText;
    public TextMeshProUGUI currentBowLevelText;
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI currentHealthLevelText;
    public Button upgradeBowButton;
    public Button upgradeHealthButton;

    public InventoryManager inventory;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        upgradeBowButton.onClick.AddListener(OnUpgradeBowButtonPressed);
        upgradeHealthButton.onClick.AddListener(OnUpgradeHealthButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        bowCostText.text = "Cost: \n" + shop.currentBowUpgradeCost;
        healthCostText.text = "Cost: \n" + shop.currentHealthUpgradeCost;
        currentBowLevelText.text = "Bow lvl: \n" + player.currentBowLvl;
        currentHealthLevelText.text = "Health lvl: \n" + player.currentHealthLvl;
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
            shop.Upgrade("Bow");
            
        }
        else return;
    }

     public void OnUpgradeHealthButtonPressed()
    {
        if (shop.canUpgradeHealth)
        {
            print("Health Upgraded");
            inventory.RemoveItems("blueberry",shop.currentHealthUpgradeCost);
            shop.Upgrade("Health");
        }
        else return;
    }

}
