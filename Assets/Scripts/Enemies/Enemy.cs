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

    public float speed;
    public float smoothingFactor = 0.1f; // Determines how quickly the enemy changes direction
    public PlayerAwarenessController playerAwarenessController;
    public new Rigidbody2D rigidbody;
    public Vector2 targetDirection;
    public Vector2 currentDirection = Vector2.zero;


    // Health variables
    public float health;
    public float maxHealth;
    public bool pauseAnimation;
    public float immunityTime = 0.5f;
    public UnityEngine.UI.Image healthbarFill;

    // Drop variables
    public string dropType;
    public int numberOfDrops;

    // Item prefabs
    public GameObject banana;
    public GameObject blueberry;
    public GameObject cherry;
    public Arrow arrow;

    // Item drop variabels
    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 5f;
    public float moveSpeed = 1f; // Speed at which items move away from the player
    public float maxDistanceFromEnemy = 10f; // Maximum distance from player before items stop moving


    public enum State
    {
        Left,
        Right,
        Hurt,
        Dead,
        Agro,
        Deagro,
        Idle,
        Attack
    }

    public State currentState;

    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();
        healthbarFill.fillAmount = health / maxHealth;
        pauseAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        // print(currentState);
    }

    public void UpdateState()
    {
        float distanceToTarget = Vector2.Distance(transform.position, targetDirection);

        // Check if the enemy should be in the Attack state
        if (distanceToTarget < 1.0f & playerAwarenessController.awareOfPlayer)
        {
            currentState = State.Attack;
            //StartCoroutine(PauseOtherAnimations(0.1f));
            //return;
        }

        else if (targetDirection.x > 0 && !pauseAnimation)
        {
            currentState = State.Right;
        }
        else if (targetDirection.x < 0 && !pauseAnimation)
        {
            currentState = State.Left;
        }
        else if (!pauseAnimation)
        {
            currentState = State.Idle;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Arrow"))
        {
            StartCoroutine(TakeDamage(collision));
        }
    }

    private IEnumerator TakeDamage(Collision2D collision)
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
        else
        {
            currentState = State.Hurt;
            pauseAnimation = true;
            yield return new WaitForSeconds(immunityTime);
            pauseAnimation = false;
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
            itemTransform.position += (itemTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }


    // Function to handle changes in awareness
    public void OnAwarenessChanged(bool newAwarenessState)
    {
        if (newAwarenessState)
        {
            currentState = State.Agro;
        }
        else
        {
            currentState = State.Deagro;
        }
        StartCoroutine(PauseOtherAnimations(immunityTime));
    }


    public IEnumerator PauseOtherAnimations(float time)
    {
        pauseAnimation = true;
        yield return new WaitForSeconds(time);
        pauseAnimation = false;
    }


}
