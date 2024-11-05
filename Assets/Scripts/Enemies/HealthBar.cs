using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Helpers;

public class HealthBar : MonoBehaviour
{
    public bool debug = false;
    public float maxHealth = 100;
    public float healthBarReductionSpeed = 0.1f;
    public float curHealth;
    public float desiredHealth;

    public Image healthBar;

    void Start()
    {
        curHealth = maxHealth;
        desiredHealth = curHealth;

        healthBar.fillAmount = curHealth / maxHealth;

        Print.Log(debug, "start fill amount: " + healthBar.fillAmount);
    }

    void Update()
    {
        if (curHealth > desiredHealth)
        {
            curHealth = Math.Max(0, Math.Max(desiredHealth, curHealth - healthBarReductionSpeed));
        }

        healthBar.fillAmount = curHealth / maxHealth;

        Print.Log(debug, "update fill amount: " + healthBar.fillAmount);
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public float GetDesiredHealth()
    {
        return desiredHealth;
    }

    public void SetHealthBarReductionSpeed(float healthBarReductionSpeed)
    {
        this.healthBarReductionSpeed = healthBarReductionSpeed;
    }

    public void TakeDamage(float damage)
    {
        Print.Log(debug, "healthbar take damage");
        desiredHealth = Math.Max(0, curHealth - damage);
    }
}
