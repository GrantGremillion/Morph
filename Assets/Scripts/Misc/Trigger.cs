using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(string))]
public class Trigger : MonoBehaviour
{
    private bool trigger = false;
    private Vector2 triggerDir = Vector2.zero;
    private Collider2D triggerCollider2D = null;
    public PlayerController player;
    public Enemy enemy;
    public string targetTag;
    public bool canSeePlayer = false;


    public void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        enemy = GetComponentInParent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (trigger)
        {
            Assert.IsTrue(triggerCollider2D != null);
            triggerDir = triggerCollider2D.transform.position - transform.position;

            Debug.DrawRay(enemy.transform.position, triggerDir *10f, Color.red);

            int layerMask = LayerMask.GetMask("Player","Default");

            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position,triggerDir, 10f, layerMask);

            
            if (hit)
            {
                if (hit.transform.GetComponentInChildren<SpriteRenderer>() != null)
                {
                    hit.transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }

                // Check if the raycast hit the player
                if (hit.collider.CompareTag("Player"))
                {
                    canSeePlayer = true; // Player is visible
                    print("Can see player");
                }
                else
                {
                    canSeePlayer = false; // Something else is blocking the view
                    print("Can't see player");
                }
            }
            else
            {
                canSeePlayer = false; // Raycast hit nothing
                print("No object in raycast path");
            }
        }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            triggerCollider2D = other;
            triggerDir = triggerCollider2D.transform.position - transform.position;
            trigger = true;
        } 

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            //print("trigger exit");
            triggerCollider2D = other;
            triggerDir = Vector2.zero;
            trigger = false;
        }
    }

    public bool getTrigger()
    {
        return trigger;
    }

    public Vector2 getTriggerDir()
    {
        return triggerDir;
    }

    public Collider2D getTriggerCollider2D()
    {
        return triggerCollider2D;
    }
}
