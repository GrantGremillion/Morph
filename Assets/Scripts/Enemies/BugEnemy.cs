using System;
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
    public Transform spriteTransform;

    public bool playerAwarenessRadiusIsTriggered = false;
    public bool attackRadiusIsTriggered = false;
    public float animationSpeed;
    
    public float attackCooldown;
    private float initialAttackCooldown;
    public float projectileSpeed = 1.0f;
    
    public Transform attackPoint;
    public Animator animator;

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
        PlayAnimations();
    }

    private void CheckIfCanAttack()
    {
        attackRadiusIsTriggered = attackRadius.getTrigger();
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();

        if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;

            if (attackCooldown <= 0 && attackRadiusIsTriggered && !isAttacking) 
            {
            canAttack = true;
            }
            else canAttack = false;
    }

    private void UpdateTargetDirection()
    {
        if (playerAwarenessRadiusIsTriggered && !pauseAnimation)
        {
            targetDirection = playerAwarenessRadius.getTriggerDir().normalized;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rigidbody.velocity = Vector2.zero;
        }

        switch (currentState)
        {
            case State.Up:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case State.Down:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case State.Right:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case State.Left:
                rigidbody.velocity = targetDirection * speed;
                spriteTransform.rotation = Quaternion.Euler(0, 0, 90);
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
            currentState = State.Attack;
            isAttacking = true;
            ThrowProjectile();
            attackCooldown = initialAttackCooldown;
        }

        if (!pauseAnimation && !isAttacking)
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

        Rigidbody2D quillRigidbody = projectileInstance.GetComponent<Rigidbody2D>();
        if (quillRigidbody != null)
        {
            quillRigidbody.velocity = direction * projectileSpeed;
        }

        // Trigger an animation event or coroutine to reset `isAttacking` after a delay
        Invoke("EndAttack", 0.5f); // Adjust the delay to match the attack animation duration
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    void PlayAnimations()
    {
        if (currentState != State.Idle)
        {
            animator.Play("Walk");
        }
        else
        {
            animator.Play("Idle");
        }
    }
}
