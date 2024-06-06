using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRadius : MonoBehaviour
{

    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        
        if (collider.CompareTag("Player"))
        {
            enemy.canAttack = true;
        }
    }

    void OnTriggerExit2D (Collider2D collider)
    {
        
        if (collider.CompareTag("Player"))
        {
            enemy.canAttack = false;
        }
    }
}
