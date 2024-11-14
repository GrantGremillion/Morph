using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BugEnemy : Enemy
{
    public PlayerController player;
    public new Collider2D collider;
    private Trigger attackRadius;
    private Trigger playerAwarenessRadius;
    public GameObject bugProjectilePrefab;
    public Transform spriteTransform;

    public bool playerAwarenessRadiusIsTriggered = false;
    public bool attackRadiusIsTriggered = false;
    public float animationSpeed;
    
    public float attackCooldown;
    private float initialAttackCooldown;
    public float projectileSpeed = 1.0f;
    private float changeDirectionCooldown;
    
    public Transform attackPoint;

    protected override void Awake()
    {
        // Assuming the SpriteRenderer is a child of the MushroomEnemy GameObject
        Transform child = transform.GetChild(3);
        spriteRenderer = child.GetComponent<SpriteRenderer>();
        base.Awake();
        targetDirection = transform.up;
    }

    void Start()
    {
        initialAttackCooldown = attackCooldown;
        itemDropType = "blueberry";
        player = FindAnyObjectByType<PlayerController>();
        collider = GetComponent<Collider2D>();

        attackRadius = transform.Find("AttackRadius").GetComponent<Trigger>();
        playerAwarenessRadius = transform.Find("PlayerAwarenessRadius").GetComponent<Trigger>();
    }

    private void FixedUpdate()
    {
        //print(GameManager.Instance.CurrentGameState);
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) 
        {   rigidbody.velocity = Vector2.zero;
            return;
        }
        player = FindAnyObjectByType<PlayerController>();

        CheckIfCanAttack();
        UpdateTargetDirection();
        SetVelocity();
    }

    private void CheckIfCanAttack()
    {
        attackRadiusIsTriggered = attackRadius.getTrigger();

        if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;

            if (attackCooldown <= 0 && attackRadiusIsTriggered) 
            {
            canAttack = true;
            }
            else canAttack = false;
    }


    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
    }

    private void HandleRandomDirectionChange()
    {
        changeDirectionCooldown -= Time.fixedDeltaTime;

        if (changeDirectionCooldown <= 0 || hitWall)
        {
            if (hitWall) targetDirection = -targetDirection;
            else
            {
                float angleChange = Random.Range(-90f,90f);
                Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                targetDirection = rotation * targetDirection;
            }

            changeDirectionCooldown = Random.Range(1f, 5f);
            hitWall = false;
        }
    }

    private void HandlePlayerTargeting()
    {
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();
        if (playerAwarenessRadiusIsTriggered)
        {
            targetDirection = playerAwarenessRadius.getTriggerDir().normalized;
            speed = originalSpeed;
        }

        speed = idleSpeed;
    }

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rigidbody.velocity = Vector2.zero;
            healthBar.gameObject.SetActive(false);
        }

        switch (currentState)
        {
            case State.Up:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
                healthBar.gameObject.SetActive(true);
                break;
            case State.Down:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, 180);
                healthBar.gameObject.SetActive(true);
                break;
            case State.Right:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, -90);
                healthBar.gameObject.SetActive(true);
                break;
            case State.Left:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, 90);
                healthBar.gameObject.SetActive(true);
                break;
            case State.Dead:
                rigidbody.velocity = Vector2.zero;
                break;
        }
    }

    public override void UpdateState()
    {
        if (canAttack && !isAttacking)
        {
            isAttacking = true;
            ThrowProjectile();
            attackCooldown = initialAttackCooldown;
        }

        if (!isAttacking)
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");

            if (Math.Abs(targetDirection.y) > Math.Abs(targetDirection.x) && targetDirection.y > 0)
            {
                currentState = State.Up;
            }
            else if (Math.Abs(targetDirection.y) > Math.Abs(targetDirection.x) && targetDirection.y < 0)
            {
                currentState = State.Down;
            }
            else if (Math.Abs(targetDirection.y) < Math.Abs(targetDirection.x) && targetDirection.x > 0)
            {
                currentState = State.Right;
            }
            else if (Math.Abs(targetDirection.y) < Math.Abs(targetDirection.x) && targetDirection.x < 0)
            {
                currentState = State.Left;
            }
            else if (!isAttacking)
            {
                currentState = State.Idle;
                gameObject.layer = LayerMask.NameToLayer("IdleEnemy");
            }
        }

        if (!canAttack)
        {
            isAttacking = false;
        }
    }

    public void ThrowProjectile()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        GameObject projectileInstance = Instantiate(bugProjectilePrefab, attackPoint.position, Quaternion.identity);

        Rigidbody2D projectileRigidbody = projectileInstance.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.velocity = direction * projectileSpeed;
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Untagged") && !awareOfPlayer)
        {
            hitWall = true;
        }
    }
}
