using Unity.VisualScripting;
using UnityEngine;

public class Porcupine : Enemy
{
    public PlayerController player;
    //public Animator animator;

    public SpriteRenderer spriteRenderer;
    public new Collider2D collider;
    private Trigger attackRadius;
    private Trigger playerAwarenessRadius;
    public GameObject quillPrefab;

    public bool playerAwarenessRadiusIsTriggered = false;
    public bool attackRadiusIsTriggered = false;
    [SerializeField] private float animationSpeed;
    [SerializeField] private float circleRadius = 2.0f; // Radius of the circling path
    [SerializeField] private float circleSpeed = 1.0f; // Speed of circling
    [SerializeField] private float directionChangeInterval = 3.0f; // Interval for changing circling direction

    private bool isCircling = false;
    private float angle = 0f; // Current angle for circling
    private float jumpCooldownTimer = 0f;
    private float directionChangeTimer = 0f;
    private bool isClockwise = false; // Flag to store circling direction
    private float attackCooldown = 1.0f;
    public float quillSpeed = 1.0f;
    public bool hasThrownProjectile;

    // Start is called before the first frame update
    void Start()
    {
        dropType = "blueberry";
        maxHealth = 15;
        player = FindAnyObjectByType<PlayerController>();
        //animator = GetComponent<Animator>();
        //animator.speed = animationSpeed;
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

        // Initialize the circling direction randomly
        isClockwise = Random.value > 0.5f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        player = FindAnyObjectByType<PlayerController>(); // Update the player reference
        attackRadiusIsTriggered = attackRadius.getTrigger();
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();

        if (jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.fixedDeltaTime;
        }

        if (attackRadiusIsTriggered)
        {
            isCircling = true;
            CirclingAroundPlayer();
            directionChangeTimer += Time.fixedDeltaTime;

            // Change circling direction at specified intervals
            if (directionChangeTimer >= directionChangeInterval)
            {
                directionChangeTimer = 0f;
                isClockwise = Random.value > 0.5f;
            }
        }
        else
        {
            isCircling = false;
        }

        if (!isCircling)
        {
            UpdateTargetDirection();
        }

        SetVelocity();

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

    private void UpdateTargetDirection()
    {
        if (playerAwarenessRadiusIsTriggered && !pauseAnimation)
        {
            targetDirection = playerAwarenessRadius.getTriggerDir().normalized;
            canAttack = true;
        }
        else
        {
            targetDirection = Vector2.zero;
            canAttack = false;
        }
    }

    private void SetVelocity()
    {
        if (currentState != State.Attack)
        {
            if (isCircling)
            {
                // Calculate the target position on the circular path
                Vector2 playerPosition = player.transform.position;
                Vector2 circlePosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;

                // Smoothly interpolate towards the target position
                Vector2 targetVelocity = (circlePosition - (Vector2)rigidbody.position).normalized * circleSpeed;
                rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, targetVelocity, Time.fixedDeltaTime * circleSpeed * 2); // Adjust the interpolation speed as needed
            }
            else if (targetDirection == Vector2.zero)
            {
                rigidbody.velocity = Vector2.zero;
            }
            else if (currentState == State.Up)
            {
                rigidbody.velocity = targetDirection * speed;
                spriteRenderer.flipY = false;
            }
            else if (currentState == State.Down)
            {
                rigidbody.velocity = targetDirection * speed;
                spriteRenderer.flipY = true;
            }
            else if (currentState == State.Dead)
            {
                rigidbody.velocity = Vector2.zero;
            }
        }

    }


    private void CirclingAroundPlayer()
    {
        float deltaAngle = isClockwise ? -circleSpeed * Time.fixedDeltaTime : circleSpeed * Time.fixedDeltaTime;
        angle += deltaAngle;
        if (angle >= 2 * Mathf.PI)
        {
            angle -= 2 * Mathf.PI;
        }
        else if (angle < 0)
        {
            angle += 2 * Mathf.PI;
        }
    }

    // void PlayAnimations()
    // {
    //     switch (currentState)
    //         {
    //             case State.Attack:
    //                 animator.Play("Attack");
    //                 break;
    //             case State.Left:
    //                 animator.Play("Walk");
    //                 break;
    //             case State.Right:
    //                 animator.Play("Walk");
    //                 break;
    //             case State.Hurt:
    //                 animator.Play("Hurt");
    //                 break;
    //             case State.Dead:
    //                 animator.Play("Dead");
    //                 break;
    //             case State.Deagro:
    //                 animator.Play("Root");
    //                 break;
    //             case State.Agro:
    //                 animator.Play("Uproot");
    //                 break;
    //             case State.Idle:
    //                 animator.Play("Idle");
    //                 break;
    //         }
    // }

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
            attackCooldown = 1.0f; // Reset the cooldown timer
            hasThrownProjectile = true; // Set the flag to indicate that a projectile has been thrown
        }

        // Reset the flag if the enemy is not attacking
        if (!canAttack)
        {
            hasThrownProjectile = false;
        }

        if (!pauseAnimation)
        {
            if (targetDirection.y > 0)
            {
                currentState = State.Up;
            }
            else if (targetDirection.y < 0)
            {
                currentState = State.Down;
            }
            else
            {
                currentState = State.Idle;
            }
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
        GameObject quillInstance = Instantiate(quillPrefab, offsetPosition, Quaternion.identity);

        // Get the Rigidbody2D component of the projectile
        Rigidbody2D quillRigidbody = quillInstance.GetComponent<Rigidbody2D>();
        if (quillRigidbody != null)
        {
            // Set the velocity of the projectile to move towards the player's current position
            quillRigidbody.velocity = direction * quillSpeed;

            // Debug log to check projectile velocity
            Debug.Log("Projectile Velocity: " + quillRigidbody.velocity);
        }
        else
        {
            Debug.LogError("Quill prefab does not have a Rigidbody2D component.");
        }
    }



}
