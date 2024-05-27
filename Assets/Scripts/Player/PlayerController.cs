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

    // Shooting variables
    private bool canShoot = true;
    public float holdTimeToShoot = 0.5f; // Time in seconds to hold the button to shoot
    public float shootCooldown = 1.0f;
    private float holdTime = 0.0f;
    private float currentShootCooldown = 0.0f;
    public float arrowSpeed = 0.5f;


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


    // Shop trigger zone
    public TriggerZone tz;
    public Shop shop;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;

        // Start player facing the screen
        currentDirection = Direction.Down;

        tz = FindAnyObjectByType<TriggerZone>();
        shop = FindAnyObjectByType<Shop>();
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

        CheckPlayerInput();
        PlayAnimations();
    }


    void CheckPlayerInput()
    {

        // Left Shift = Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Open/Close shop
        if (Input.GetKeyDown(KeyCode.E) && tz.canUseShop)
        {
            if (!shop.usingShop) shop.OpenShop();
            else shop.CloseShop();
        }

        // Hold Left click
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && canDash)
        {
            holdTime += Time.deltaTime;
        }

        // Release Left click
        if (Input.GetKeyUp(KeyCode.Mouse0) && canShoot && canDash)
        {
            if (holdTime >= holdTimeToShoot)
            {
                ShootArrow();
                canShoot = false;
                holdTime = 0.0f; // Reset hold time after shooting
            }
            else
            {
                holdTime = 0.0f; // Reset hold time if not held long enough
            }
        }

        // Handle cooldown logic
        if (!canShoot)
        {
            currentShootCooldown -= Time.deltaTime;
            if (currentShootCooldown <= 0)
            {
                canShoot = true;
                currentShootCooldown = shootCooldown;
            }
        }

    }

    void ShootArrow()
{
    // Get the mouse position in world space
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0; // Ensure the z-coordinate is 0 since we're working in 2D

    // Calculate the direction from the player to the mouse position
    Vector3 direction = (mousePosition - transform.position).normalized;

    // Calculate the rotation angle
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    Quaternion rotation = Quaternion.Euler(0, 0, angle);

    // Set the velocity
    Vector2 velocity = new Vector2(direction.x, direction.y);

    // Set the offset (if needed, based on the direction)
    Vector3 offset = direction * 0.15f; // Adjust this value if needed

    // Calculate the spawn position with the offset
    Vector3 spawnPosition = transform.position + offset;

    // Instantiate the arrow at the spawn position with the calculated rotation
    GameObject arrow = Instantiate(arrowPrefab, spawnPosition, rotation);
    arrow.GetComponent<Rigidbody2D>().velocity = velocity * arrowSpeed; 
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
