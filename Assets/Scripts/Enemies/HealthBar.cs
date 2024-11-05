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
    public float primCurHealth;
    public float secCurHealth;
    public float desiredHealth;

    public Image primaryHealthBar;
    public Image secondaryHealthBar;

    void Start()
    {
        primCurHealth = maxHealth;
        secCurHealth = maxHealth;
        desiredHealth = primCurHealth;

        primaryHealthBar.fillAmount = primCurHealth / maxHealth;
        secondaryHealthBar.fillAmount = primCurHealth / maxHealth;

        Print.Log(debug, "start fill amount: " + primaryHealthBar.fillAmount);
    }

    void Update()
    {

        secCurHealth = Math.Max(0, Math.Max(desiredHealth, secCurHealth - healthBarReductionSpeed));

        primaryHealthBar.fillAmount = primCurHealth / maxHealth;
        secondaryHealthBar.fillAmount = secCurHealth / maxHealth;

        Print.Log(debug, "update fill amount: " + primaryHealthBar.fillAmount);
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
        desiredHealth = Math.Max(0, primCurHealth - damage);
        primCurHealth = desiredHealth;
    }
}
