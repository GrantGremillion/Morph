using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    private Enemy enemy;

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
        print(enemy.currentState);
        
        if (collider.CompareTag("Arrow") && enemy.currentState == Enemy.State.Idle)
        {
            Destroy(collider.gameObject);
        }
    }


}
