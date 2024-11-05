using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    [HideInInspector] PlayerController player;

    [SerializeField] private int playerSlowdownTime;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") player.WebProjectileCollision(playerSlowdownTime);
        Destroy(gameObject);
    }
}
