using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool awareOfPlayer { get; private set; }
    public Vector2 directionToPlayer { get; private set; }

    [SerializeField] private float playerAwarenessDistance;

    private Transform player;
    private bool previousAwareOfPlayer;

    public Enemy enemy;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        enemy = GetComponent<Enemy>();
        previousAwareOfPlayer = awareOfPlayer;  
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;

        // Determine current awareness
        awareOfPlayer = enemyToPlayerVector.magnitude <= playerAwarenessDistance;

        // Check if awareness has changed
        if (awareOfPlayer != previousAwareOfPlayer)
        {
            enemy.OnAwarenessChanged(awareOfPlayer);
            previousAwareOfPlayer = awareOfPlayer;  
        }
    }

}
