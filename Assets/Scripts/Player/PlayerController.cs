using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Direction currentDirection;
    public InventoryManager inventory;
    public GameObject arrowPrefab;
    public Transform bowTransform;
    public Bow bow;
    public UpgradeSystem upgradeSystem;

    public bool canPlay = false;

    // Shooting variables
    public bool canShoot = true;
    public float holdTimeToShoot; // Time in seconds to hold the button to shoot
    public float shootCooldown = 0.5f;
    private float holdTime = 0.0f;


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
    public float speed { get; set;}
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime = 0.2f;


    // Shop trigger zone
    public TriggerZone tz;
    public Shop shop;

    // Upgrade Levels
    public int currentBowLvl;
    public int currentHealthLvl;

    // Settings variables
    private bool canMenu = true;

    // Morph variables
    [SerializeField] private bool canMorphToFish = false;
    private bool touchingWater = false;
    [SerializeField] FishController fish;


    // Player Sound Effects
    [SerializeField] private AudioClip takeDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        upgradeSystem = GetComponentInChildren<UpgradeSystem>();
        animator.speed = animationSpeed;

        // Start player facing the screen
        currentDirection = Direction.Down;

        tz = FindAnyObjectByType<TriggerZone>();
        shop = FindAnyObjectByType<Shop>();

        speed = 0.6f;

        currentBowLvl = 0;
        currentHealthLvl = 0;

        bow = bowTransform.gameObject.GetComponent<Bow>();
        holdTimeToShoot = 0.5f;
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Main" && !canPlay) return;

        if (isDashing || isKnockedBack)
        {
            return;
        }

        rb.velocity = movementInput.normalized * speed;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main" && !canPlay) return;

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
            print("Pressing E");
            if (!shop.usingShop) shop.OpenShop();
            else shop.CloseShop();
        }

        // Open/Close settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (canMenu){
                print("Entering the Menu");
                canMenu = false;
            }
            else{
                print("Exiting the Menu");
                canMenu = true;
            }
        }

        // morph
        if (Input.GetKeyUp(KeyCode.M))
        {
            changeForm();
        }

        // Hold Left click
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && canDash)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdTimeToShoot)
            {
                if (bow.animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") == false)
                {
                    bow.animator.Play("Shoot");
                }
            }
            else if (bow.animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") == false)
            {
                bow.animator.Play("Shoot");
            }
        }

        // Release Left click
        if (Input.GetKeyUp(KeyCode.Mouse0) && canShoot && canDash)
        {
            //print("Hold Time" + holdTimeToShoot);

            // Bow can be shot
            if (holdTime >= holdTimeToShoot)
            {
                ShootArrow();
                if (bow.animator.GetCurrentAnimatorStateInfo(0).IsName("Still") == false)
                {
                    bow.animator.Play("Still");
                }
                holdTime = 0.0f; // Reset hold time after shooting
            }
            else
            {
                if (bow.animator.GetCurrentAnimatorStateInfo(0).IsName("Still") == false)
                {
                    bow.animator.Play("Still");
                }
                holdTime = 0.0f; // Reset hold time if not held long enough
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
        arrow.GetComponent<Rigidbody2D>().velocity = velocity * arrow.GetComponent<Arrow>().speed; ;
    }

    void changeForm()
    {
        if (canMorphToFish == true && touchingWater == true)
        {
            SceneManager.LoadScene(2);
        }
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

                bowTransform.SetParent(null);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                bowTransform.SetParent(transform);

            }
            else if (movementInput.x < 0)
            {
                animator.Play("WalkHorizontal");
                currentDirection = Direction.Left;

                bowTransform.SetParent(null);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                bowTransform.SetParent(transform);
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
        if (collision.gameObject.CompareTag("Water"))
        {
            touchingWater = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            touchingWater = false;
        }
    }

    public IEnumerator Knockback(Collider2D collision)
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

    public void TakeDamage(Collider2D collider)
    {
        health--;
        SoundFXManager.instance.PlaySoundFXClip(takeDamage, transform, 1f, false);
        StartCoroutine(Knockback(collider));
    }




}
