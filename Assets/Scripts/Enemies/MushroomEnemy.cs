
using Unity.VisualScripting;
using UnityEngine;

public class MushroomEnemy : Enemy
{

    public Animator animator;

    public Canvas healthbar;

    public Collider2D collider;

    [SerializeField] private float animationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        dropType = "banana";
        maxHealth = 10;

        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;

        healthbar = GetComponentInChildren<Canvas>();
        collider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
        PlayAnimations();

        if (currentState == State.Idle)
        {
            collider.enabled = false;
        }
        else collider.enabled = true;
    }


    private void UpdateTargetDirection()
    {
        if (playerAwarenessController.awareOfPlayer)
        {
            targetDirection = playerAwarenessController.directionToPlayer;
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
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (currentState == State.Left)
        {
            rigidbody.velocity = targetDirection * speed;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

}
