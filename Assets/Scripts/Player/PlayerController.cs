using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Direction currentDirection;
    public InventoryManager inventory;
    public GameObject arrowPrefab;


    // Dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 1f;

    // Health variables
    public int health;
    public int numOfHearts;

    // Knockback variables
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    private bool isKnockedBack;

    // Facing Directions
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private float animationSpeed;
    [SerializeField] private float speed;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;

        // Start player facing the screen
        currentDirection = Direction.Down;
    }
    void FixedUpdate()
    {
        if (isDashing || isKnockedBack)
        {
            return;
        }

        rb.velocity = movementInput.normalized * speed;
    }

    void Update()
    {
        if (isDashing || isKnockedBack)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Left mouse = Shoot arrow
        if (Input.GetKeyDown(KeyCode.Mouse0) && canDash)
        {
            Quaternion rotation = Quaternion.identity;
            Vector2 velocity = Vector2.zero;
            Vector3 offset = Vector3.zero;
            float offsetDistance = .15f; // Distance to offset the arrow from the player

            switch (currentDirection)
            {
                case Direction.Up:
                    rotation = Quaternion.Euler(0, 0, 90);
                    velocity = new Vector2(0.0f, 1.0f);
                    offset = new Vector3(0, offsetDistance, 0);
                    break;
                case Direction.Down:
                    rotation = Quaternion.Euler(0, 0, -90);
                    velocity = new Vector2(0.0f, -1.0f);
                    offset = new Vector3(0, -offsetDistance, 0);
                    break;
                case Direction.Left:
                    rotation = Quaternion.Euler(0, 0, 180);
                    velocity = new Vector2(-1.0f, 0.0f);
                    offset = new Vector3(-offsetDistance, 0, 0);
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 0, 0);
                    velocity = new Vector2(1.0f, 0.0f);
                    offset = new Vector3(offsetDistance, 0, 0);
                    break;
            }

            // Calculate the spawn position with the offset
            Vector3 spawnPosition = transform.position + offset;

            // Instantiate the arrow at the spawn position with the calculated rotation
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, rotation);
            arrow.GetComponent<Rigidbody2D>().velocity = velocity;
        }

        PlayAnimations();

    }

    void PlayAnimations()
    {
        if (movementInput == Vector2.zero)
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    animator.Play("IdleUp");
                    break;
                case Direction.Down:
                    animator.Play("IdleDown");
                    break;
                case Direction.Left:
                    animator.Play("IdleHorizontal");
                    break;
                case Direction.Right:
                    animator.Play("IdleHorizontal");
                    break;
            }
        }
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();

        if (movementInput != Vector2.zero)
        {
            // Prioritize horizontal movement over vertical movement
            if (movementInput.x > 0)
            {
                animator.Play("WalkHorizontal");
                currentDirection = Direction.Right;

                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (movementInput.x < 0)
            {
                animator.Play("WalkHorizontal");
                currentDirection = Direction.Left;

                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (movementInput.y > 0)
            {
                animator.Play("WalkUp");
                currentDirection = Direction.Up;
            }
            else if (movementInput.y < 0)
            {
                animator.Play("WalkDown");
                currentDirection = Direction.Down;
            }

            //Debug.Log("Current Direction: " + currentDirection);
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        if (currentDirection == Direction.Left)
        {
            rb.velocity = new Vector2(-dashingPower, 0f);
        }
        else if (currentDirection == Direction.Right)
        {
            rb.velocity = new Vector2(dashingPower, 0f);
        }
        else if (currentDirection == Direction.Up)
        {
            rb.velocity = new Vector2(0f, dashingPower);
        }
        else
        {
            rb.velocity = new Vector2(0f, -dashingPower);
        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health--;
            StartCoroutine(Knockback(collision));
        }
    }

    private IEnumerator Knockback(Collision2D collision)
    {
        isKnockedBack = true;

        Vector2 difference = (transform.position - collision.transform.position).normalized;
        Vector2 force = difference * knockbackForce;
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;
        // Wait until player has been knowcked back to drop items
        inventory.DropItems();
    }




}
