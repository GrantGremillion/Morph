using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Search;
using UnityEditor.XR;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public bool usingShop = false;

    public ShopUI shopUI;
    public PlayerController player;
    public Bow bow;
    public WeaponManager weaponManager;


    // Player item counts
    public int bananaCount;
    public int blueberryCount;
    public int cherryCount;


    // Upgrade variables
    public List<int> bowUpgradeCosts;
    public int currentBowUpgradeCost;
    public bool canUpgradeBow;
    public List<int> healthUpgradeCosts;
    public int currentHealthUpgradeCost;
    public bool canUpgradeHealth;


    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        bow = player.GetComponentInChildren<Bow>();
        shopUI = FindAnyObjectByType<ShopUI>();

        if (shopUI != null)
        {
            shopUI.gameObject.SetActive(false);
        }

        bowUpgradeCosts = new List<int> { 1, 2, 3, 4, 5 };
        healthUpgradeCosts = new List<int> { 1, 2, 3, 4, 5 };
    }

    // Update is called once per frame
    void Update()
    {
        if (usingShop)
        {
            //GameManager.Instance.CurrentGameState = GameManager.GameState.Paused;
            GetPlayerItemCounts();
            shopUI.UpdateUI();
            CompareCounts();
            GetUpgradeCosts();
        }
    }

    public void OpenShop()
    {
        print("shop");
        usingShop = true;
        if (shopUI != null)
        {
            weaponManager.canShoot = false;
            shopUI.gameObject.SetActive(true);
        }
    }

    public void CloseShop()
    {
        usingShop = false;
        weaponManager.canShoot = true;
        if (shopUI != null)
        {
            shopUI.gameObject.SetActive(false);
        }

    }

    public void GetPlayerItemCounts()
    {
        bananaCount = player.inventory.GetItemCount("banana");
        blueberryCount = player.inventory.GetItemCount("blueberry");
        cherryCount = player.inventory.GetItemCount("cherry");

        //print("Bananas: " + bananaCount + "Blueberries: " + blueberryCount + "Cherries: " + cherryCount);
    }

    public void GetUpgradeCosts()
    {
        // Get costs for all upgrades in shop based on players current level in each category
        currentBowUpgradeCost = bowUpgradeCosts[player.currentBowLvl];
        currentHealthUpgradeCost = healthUpgradeCosts[player.currentHealthLvl];
    }

    public void CompareCounts()
    {
        // Check if player has enough for each upgrade

        if (bananaCount >= currentBowUpgradeCost)
        {
            canUpgradeBow = true;
        }
        else canUpgradeBow = false;


        if (blueberryCount >= currentHealthUpgradeCost)
        {
            canUpgradeHealth = true;
        }
        else canUpgradeHealth = false;
    }

    public void Upgrade(string upgradeType)
    {
        if (upgradeType == "Bow")
        {
            player.currentBowLvl++;
            if (player.currentBowLvl % 4 == 0)
            {
                bow.UpdateSprite();
            }
        }
        else if (upgradeType == "Health")
        {
            player.currentHealthLvl++;
            player.numOfHearts++;
        }

    }

}
