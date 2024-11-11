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
    public UpgradeSystem upgradeSystem;
    public WeaponManager weaponManager;

    // Shooting variables
    public float holdTimeToShoot; // Time in seconds to hold the button to shoot
    public float shootCooldown = 0.5f;

    // Dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 0f;

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


    // Player Sound Effects
    [SerializeField] private AudioClip takeDamage;

    private bool slowed;

    private bool paused;

    [SerializeField]
    private UIManager uiManager;

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
        slowed = false;

        currentBowLvl = 0;
        currentHealthLvl = 0;
    }
    void FixedUpdate()
    {
        //print(GameManager.Instance.CurrentGameState);
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return;
        if (isDashing || isKnockedBack) return;
        
        rb.velocity = movementInput.normalized * speed;
    }

    void Update()
    {
        CheckPlayerPause();
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return;
        CheckPlayerInput();
        PlayAnimations();
    }

    void CheckPlayerPause()
    {
        // Open/Close settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)  
            {
                //print ("Pause"); 
                GameManager.Instance.PauseGame();
                paused = true;
                uiManager.PauseMenuUI(true);
            }
            else 
            {
                //print ("Unpause");
                GameManager.Instance.StartGame();
                paused = false;
                uiManager.PauseMenuUI(false);
            }
        }
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponManager.SwitchToWeapon("Bow");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponManager.SwitchToWeapon("ThrowingStar");
        }


        weaponManager.CheckWeaponInput(canDash);
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

        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return;

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
        //inventory.DropItems();
    }

    public void TakeDamage(Collider2D collider)
    {
        health--;
        SoundFXManager.instance.PlaySoundFXClip(takeDamage, transform, 1f, false);
        StartCoroutine(Knockback(collider));
    }

    public void WebProjectileCollision(int slowedTime)
    {
        if (!slowed)
    {
        StartCoroutine(SlowDown(slowedTime));
    }
    }

    public IEnumerator SlowDown(int slowedTime)
    {
        float originalSpeed = speed; 
        speed = originalSpeed * 0.5f; 
        slowed = true; 

        yield return new WaitForSeconds(slowedTime); 

        speed = originalSpeed; 
        slowed = false; 
    }
}
