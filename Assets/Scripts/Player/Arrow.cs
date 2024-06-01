using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public PlayerController player;
    public UpgradeSystem upgradeSystem; 


    public float destroyTime = 1.0f;
    public float damage;
    public float speed;



    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        upgradeSystem = player.GetComponentInChildren<UpgradeSystem>();

        damage = upgradeSystem.arrowDamages[player.currentBowLvl];
        speed = upgradeSystem.arrowSpeeds[player.currentBowLvl];
        print("Player's Bow Level: " + player.currentBowLvl);
        print("Arrow Damage: " + damage);
        print("Arrow Speed: " + speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(DestroyAfterDelay(destroyTime)); 
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
