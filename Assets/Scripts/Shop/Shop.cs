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

    public int bananaCount;
    public int blueberryCount;
    public int cherryCount;

    public List<string> upgrades;

    public List<int> bowUpgradeCosts;
    public int currentBowUpgradeCost;
    public bool canUpgradeBow;
    public List<int> healthUpgradeCosts;
    public int currentHealthUpgradeCost;
    public bool canUpgradeHealth;



    public int levels;
    public int maxLevels;


    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        shopUI.gameObject.SetActive(false);

        bowUpgradeCosts = new List<int> { 10, 20, 30 };
        healthUpgradeCosts = new List<int> { 10, 20, 30 };

        upgrades = new List<string> { "Bow", "Health" };
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerItemCounts();
        shopUI.UpdateUI();
        CompareCounts();
        GetUpgradeCosts();
    }

    public void OpenShop()
    {
        usingShop = true;
        if (shopUI != null)
        {
            player.canShoot = false;
            shopUI.gameObject.SetActive(true);
        }
    }

    public void CloseShop()
    {
        usingShop = false;
        player.canShoot = true;
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

    public void Upgrade()
    {
        levels++;
        player.currentBowLvl++;
    }

}
