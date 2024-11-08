using System;
using UnityEngine;

public class BugEnemy : Enemy
{
    public PlayerController player;
    public SpriteRenderer spriteRenderer;
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
    
    public Transform attackPoint;

    void Start()
    {
        initialAttackCooldown = attackCooldown;
        itemDropType = "blueberry";
        player = FindAnyObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();
        if (playerAwarenessRadiusIsTriggered)
        {
            animator.SetTrigger("Agro");
            targetDirection = playerAwarenessRadius.getTriggerDir().normalized;
        }
        else
        {
            animator.SetTrigger("Deagro");
            targetDirection = Vector2.zero;
        }
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
}
