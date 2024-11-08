using UnityEngine;
using Helpers;

public class MushroomEnemy : Enemy
{
    // Object references
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;
    private Trigger attackRadius;
    private Trigger playerAwarenessRadius;

    public bool playerAwarenessRadiusIsTriggered = false;
    public bool attackRadiusIsTriggered = false;
    public float animationSpeed;

    void Start()
    {
        itemDropType = "banana";
        player = FindAnyObjectByType<PlayerController>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        attackRadius = transform.Find("AttackRadius").GetComponent<Trigger>();
        if (attackRadius == null)
        {
            Print.LogError(debug, "AttackRadius GameObject/Component not found!");
        }

        playerAwarenessRadius = transform.Find("PlayerAwarenessRadius").GetComponent<Trigger>();
        if (playerAwarenessRadius == null)
        {
            Print.LogError(debug, "playerAwarenessRadius GameObject/Component not found!");
        }
    }

    private void FixedUpdate()
    {
        //print(GameManager.Instance.CurrentGameState);
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) 
        {   rigidbody.velocity = Vector2.zero;
            return;
        }
        
        CheckIfCanAttack();
        UpdateTargetDirection();
        SetVelocity();
    }

    private void CheckIfCanAttack()
    {
        attackRadiusIsTriggered = attackRadius.getTrigger();

        if (attackRadiusIsTriggered) animator.SetTrigger("Attacking");

        else animator.SetTrigger("StopAttacking");
    }

    private void UpdateTargetDirection()
    {
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();

        if (playerAwarenessRadiusIsTriggered) targetDirection = playerAwarenessRadius.getTriggerDir().normalized;
        
        else targetDirection = Vector2.zero;
        
    }

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            animator.SetBool("Agro", false);
            animator.SetBool("Deagro", true);
            rigidbody.velocity = Vector2.zero;
        }
        else if (currentState == State.Right)
        {
            animator.SetBool("Agro", true);
            animator.SetBool("Deagro", false);
            rigidbody.velocity = targetDirection * speed;
            spriteRenderer.flipX = true;
        }
        else if (currentState == State.Left)
        {
            animator.SetBool("Agro", true);
            animator.SetBool("Deagro", false);
            rigidbody.velocity = targetDirection * speed;
            spriteRenderer.flipX = false;
        }
        else if (currentState == State.Dead)
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    public override void UpdateState()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        if (targetDirection.x > 0)
        {
            currentState = State.Right;
        }
        else if (targetDirection.x < 0)
        {
            currentState = State.Left;
        }
        else
        {
            currentState = State.Idle;
            gameObject.layer = LayerMask.NameToLayer("IdleEnemy");
        }
        
    }

    public void DealDamage()
    {
        player.TakeDamage(collider);
    }

}
