using Unity.VisualScripting;
using UnityEngine;

public class BugEnemy : Enemy
{
    public PlayerController player;
    public SpriteRenderer spriteRenderer;
    public new Collider2D collider;
    private Trigger attackRadius;
    private Trigger playerAwarenessRadius;
    public GameObject bugProjectilePrefab;

    public bool playerAwarenessRadiusIsTriggered = false;
    public bool attackRadiusIsTriggered = false;
    [SerializeField] private float animationSpeed;
    
    [SerializeField]
    private float attackCooldown;
    private float initialAttackCooldown;
    public float projectileSpeed = 1.0f;
    public bool hasThrownProjectile;

    // Start is called before the first frame update
    void Start()
    {
        initialAttackCooldown = attackCooldown;
        dropType = "blueberry";
        player = FindAnyObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        attackRadius = transform.Find("AttackRadius").GetComponent<Trigger>();
        if (attackRadius == null)
        {
            Debug.LogError("AttackRadius GameObject/Component not found!");
        }

        playerAwarenessRadius = transform.Find("PlayerAwarenessRadius").GetComponent<Trigger>();
        if (playerAwarenessRadius == null)
        {
            Debug.LogError("playerAwarenessRadius GameObject/Component not found!");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        player = FindAnyObjectByType<PlayerController>(); // Update the player reference
        attackRadiusIsTriggered = attackRadius.getTrigger();
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.fixedDeltaTime;
        }

        // Ensure that the enemy can only attack when the cooldown is finished
        if (attackCooldown <= 0 && attackRadiusIsTriggered)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    // Called via event set up in attack animation 
    public void DealDamage()
    {
        player.TakeDamage(collider);
    }

    // Called in base Class Update function
    public override void UpdateState()
    {
        // Check if the enemy should be in the Attack state
        if (canAttack && !hasThrownProjectile)
        {
            currentState = State.Attack;
            ThrowProjectile();
            StartCoroutine(PauseOtherAnimations(0.1f));
            currentState = State.None; // Reset state after attacking
            attackCooldown = initialAttackCooldown; // Reset the cooldown timer
            hasThrownProjectile = true; // Set the flag to indicate that a projectile has been thrown
        }

        // Reset the flag if the enemy is not attacking
        if (!canAttack)
        {
            hasThrownProjectile = false;
        }
    }

    public void ThrowProjectile()
    {
        // Ensure to fetch the current position of the player at the time of instantiation
        Vector2 playerPosition = player.transform.position;
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        // Offset position to instantiate the projectile slightly in front of the enemy
        Vector2 offsetPosition = (Vector2)transform.position + direction * 0.2f; // Adjust the offset distance as needed

        // Instantiate the Quill projectile at the offset position
        GameObject quillInstance = Instantiate(bugProjectilePrefab, offsetPosition, Quaternion.identity);

        // Get the Rigidbody2D component of the projectile
        Rigidbody2D quillRigidbody = quillInstance.GetComponent<Rigidbody2D>();
        if (quillRigidbody != null)
        {
            // Set the velocity of the projectile to move towards the player's current position
            quillRigidbody.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Quill prefab does not have a Rigidbody2D component.");
        }
    }
}
