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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            //print("trigger enter");
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
