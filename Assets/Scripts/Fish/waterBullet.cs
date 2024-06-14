using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10.0f;
    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // destroy after lifeTime
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }
}
