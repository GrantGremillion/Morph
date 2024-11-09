using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    public AnimatorOverrideController overrideController; // Used to update weapon sprites
    public PlayerController player;
    public Transform playerTransform;
    public UpgradeSystem upgradeSystem; 
    public String type;


}
