using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public new Rigidbody2D rigidbody;
    [HideInInspector]
    public Vector2 targetDirection;
    [HideInInspector]
    public Vector2 currentDirection = Vector2.zero;
    [HideInInspector]
    public UnityEngine.UI.Image healthbarFill;

    
    // Item prefabs
    public GameObject banana;
    public GameObject blueberry;
    public GameObject cherry;
    public Arrow arrow;

    public State currentState;


    public float speed;
    public float health;
    public float maxHealth;
    public float immunityTime = 0.5f;
    public bool pauseAnimation;
    
    // Item drop variabels
    public string dropType;
    public int numberOfDrops;
    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 5f;
    public float moveSpeed = 1f; // Speed at which items move away from the player
    public float maxDistanceFromEnemy = 10f; // Maximum distance from player before items stop moving

    public bool canAttack;
    private bool previousAwareOfPlayer;
    public bool awareOfPlayer { get; private set; }


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

        healthbarFill = transform.Find("Canvas").GetComponentInChildren<Transform>().Find("Fill").GetComponent<UnityEngine.UI.Image>();
        healthbarFill.fillAmount = health / maxHealth;
        pauseAnimation = false;
        canAttack = false;
        previousAwareOfPlayer = awareOfPlayer;  
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        // Needed for the enemy uproot and root animations
        if (currentState == State.Idle) awareOfPlayer = false;
        else awareOfPlayer = true;


        if (awareOfPlayer != previousAwareOfPlayer)
        {
            OnAwarenessChanged(awareOfPlayer);
            previousAwareOfPlayer = awareOfPlayer;  
        }
    }

    public virtual void UpdateState()
    {
        // Override method
    }

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

        health -= arrow.damage;
        healthbarFill.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            currentState = State.Dead;
            pauseAnimation = true;
            yield return new WaitForSeconds(immunityTime);
            DropItems();
            Destroy(gameObject);
        }
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
        // Get random distance within the min and max spawn distances
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Get random angle around the player
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate spawn position offset
        Vector3 spawnOffset = new Vector3(Mathf.Cos(randomAngle) * randomDistance, Mathf.Sin(randomAngle) * randomDistance, 0f);

        // Calculate final spawn position
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
        //print("Awarness Changed");
        if (newAwarenessState)
        {
            currentState = State.Agro;
        }
        else
        {
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
