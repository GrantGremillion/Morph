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
    [HideInInspector]
    public new Rigidbody2D rigidbody;
    [HideInInspector]
    public Vector2 targetDirection;
    [HideInInspector]
    public Vector2 currentDirection = Vector2.zero;
    [HideInInspector]
    private HealthBar healthBar;

    // Item prefabs
    public GameObject banana;
    public GameObject blueberry;
    public GameObject cherry;
    public Arrow arrow;
    public State currentState;
    public Canvas healthBarCanvas;

    public float speed;
    public float health;
    public float maxHealth;
    public float immunityTime = 0.5f;
    public bool pauseAnimation;
    
    // Item drop variables
    public string dropType;
    public int numberOfDrops;
    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 5f;
    public float moveSpeed = 1f;
    public float maxDistanceFromEnemy = 10f;

    public bool canAttack;
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
            print("swapped");
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

        print("take damage: " + arrow.damage);

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
        // Drop all enemy items
        for (int i = 0; i < numberOfDrops; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(transform.position);
            GameObject newItem = null;

            switch (dropType)
            {
                case "banana":
                    newItem = Instantiate(banana, spawnPosition, Quaternion.identity);
                    break;
                case "blueberry":
                    newItem = Instantiate(blueberry, spawnPosition, Quaternion.identity);
                    break;
                case "cherry":
                    newItem = Instantiate(cherry, spawnPosition, Quaternion.identity);
                    break;

            }

            StartCoroutine(MoveItemAway(newItem.transform));
        }
    }

    public Vector3 GetRandomSpawnPosition(Vector3 pos)
    {
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 spawnOffset = new Vector3(Mathf.Cos(randomAngle) * randomDistance, Mathf.Sin(randomAngle) * randomDistance, 0f);
        Vector3 spawnPosition = pos + spawnOffset;

        return spawnPosition;
    }

    public IEnumerator MoveItemAway(Transform itemTransform)
    {
        while (Vector3.Distance(itemTransform.position, transform.position) < maxDistanceFromEnemy)
        {
            // Move the item away from the player
            itemTransform.position += (itemTransform.position - transform.position).normalized * moveSpeed;
            yield return null;
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
