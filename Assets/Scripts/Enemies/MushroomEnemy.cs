using Unity.VisualScripting;
using UnityEngine;
using Helpers;

public class MushroomEnemy : Enemy
{
    // Object references
    private PlayerController player;
    private Animator animator;
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
        PlayAnimations();
    }

    private void CheckIfCanAttack()
    {
        attackRadiusIsTriggered = attackRadius.getTrigger();
        playerAwarenessRadiusIsTriggered = playerAwarenessRadius.getTrigger();

        if (attackRadiusIsTriggered || isAttacking)
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
        else if (currentState == State.Right)
        {
            rigidbody.velocity = targetDirection * speed;
            spriteRenderer.flipX = true;
        }
        else if (currentState == State.Left)
        {
            rigidbody.velocity = targetDirection * speed;
            spriteRenderer.flipX = false;
        }
        else if (currentState == State.Dead)
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    void PlayAnimations()
    {
        switch (currentState)
            {
                case State.Attack:
                    animator.Play("Attack");
                    break;
                case State.Left:
                    animator.Play("Walk");
                    break;
                case State.Right:
                    animator.Play("Walk");
                    break;
                case State.Hurt:
                    animator.Play("Hurt");
                    break;
                case State.Dead:
                    animator.Play("Dead");
                    break;
                case State.Deagro:
                    animator.Play("Root");
                    break;
                case State.Agro:
                    animator.Play("Uproot");
                    break;
                case State.Idle:
                    animator.Play("Idle");
                    break;
            }
    }

    // Called via event set up in attack animation 
    public void DealDamage()
    {
        player.TakeDamage(collider);
    }

    public override void UpdateState()
    {
        if (canAttack)
        {
            currentState = State.Attack;
            StartCoroutine(PauseOtherAnimations(0.1f));
        }

        if (!pauseAnimation)
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
    }

}
