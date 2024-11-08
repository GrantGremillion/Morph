using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Helpers;

public class Enemy : MonoBehaviour
{
    public bool debug = false;
    
    // Object references
    [HideInInspector] public new Rigidbody2D rigidbody;
    private HealthBar healthBar;
    public Arrow arrow;


    [HideInInspector] public Vector2 targetDirection;
    [HideInInspector] public Vector2 currentDirection = Vector2.zero;
    public State currentState;
    public Canvas healthBarCanvas;
    public float speed;
    public float health;
    public float maxHealth;
    public float immunityTime = 0.5f;
    public bool pauseAnimation;
    public string itemDropType;
    public int numberOfDrops;

    [HideInInspector] public bool canAttack;     ///
    public bool isAttacking;
    private bool previousAwareOfPlayer = false;
    public bool awareOfPlayer = false;
    public enum State
    {
        Left,
        Right,
        Hurt,
        Dead,
        Agro,
        Deagro,
        Idle,
        Attack,
        Up,
        Down,
        None
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        healthBar = healthBarCanvas.GetComponent<HealthBar>();

        healthBar.SetMaxHealth(health);
        healthBar.gameObject.SetActive(false);

        pauseAnimation = false;
        canAttack = false;
        previousAwareOfPlayer = awareOfPlayer;  
    }

    void Update()
    {
        UpdateState();

        // Needed for the enemy uproot and root animations
        if (currentState == State.Idle) awareOfPlayer = false;
        else awareOfPlayer = true;


        if (awareOfPlayer != previousAwareOfPlayer)
        {
            //print("swapped");
            OnAwarenessChanged(awareOfPlayer);
        }

        previousAwareOfPlayer = awareOfPlayer;
    }

    public virtual void UpdateState() { }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (currentState != State.Idle) StartCoroutine(TakeDamage(collision));
        }
    }

    public IEnumerator TakeDamage(Collision2D collision)
    {
        arrow = collision.gameObject.GetComponent<Arrow>();

        //print("take damage: " + arrow.damage);

        healthBar.TakeDamage(arrow.damage);
        if (healthBar.GetDesiredHealth() <= 0)
        {

            healthBar.gameObject.SetActive(false);
            currentState = State.Dead;
            pauseAnimation = true;
            yield return new WaitForSeconds(immunityTime);
            DropItems();
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(1);
    }

    public void DropItems()
    {
        for (int i = 0; i < numberOfDrops; i++)
        {
            Vector3 spawnPosition = ItemSpawner.Instance.GetRandomSpawnPosition(transform.position);
            ItemSpawner.Instance.SpawnEnemyDrops(itemDropType, spawnPosition);
        }
    }

    // Function to handle changes in awareness
    public void OnAwarenessChanged(bool newAwarenessState)
    {
        if (newAwarenessState)
        {
            healthBar.gameObject.SetActive(true);
            currentState = State.Agro;
        }
        else
        {
            healthBar.gameObject.SetActive(false);
            currentState = State.Deagro;
        }
        StartCoroutine(PauseOtherAnimations(0.2f));
    }

    public IEnumerator PauseOtherAnimations(float time)
    {
        pauseAnimation = true;
        yield return new WaitForSeconds(time);
        pauseAnimation = false;
    }
}
