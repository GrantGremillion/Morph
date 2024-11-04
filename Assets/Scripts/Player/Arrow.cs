using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public PlayerController player;
    public UpgradeSystem upgradeSystem;

    public float destroyTime = 20.0f;
    public float damage;
    public float speed;
    private bool hasCollided = false; // Flag to check if arrow has already collided

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        upgradeSystem = player.GetComponentInChildren<UpgradeSystem>();

        damage = upgradeSystem.arrowDamages[player.currentBowLvl];
        speed = upgradeSystem.arrowSpeeds[player.currentBowLvl];
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasCollided) // Only reduce destroyTime if arrow hasn't collided
        {
            destroyTime -= Time.deltaTime;

            if (destroyTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided)
        {
            hasCollided = true; // Mark as collided to prevent repeated triggers

            // Stop the arrow's movement by setting its Rigidbody2D velocity to zero
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true; // Makes the arrow stick to the target
            }

            // Disable the collider to prevent further collisions
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Make the arrow a child of the object it hit to ensure it sticks
            transform.parent = collision.transform;

            // Start the timer to destroy the arrow after 5 seconds
            StartCoroutine(DestroyAfterDelay(5.0f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
