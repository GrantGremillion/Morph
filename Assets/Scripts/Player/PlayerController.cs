using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Direction currentDirection;


    // Dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingCooldown = 1f;

    // Health variables
    public int health;
    public int numOfHearts;

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

    [SerializeField] private float speed;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();

        if (movementInput != Vector2.zero)
        {
            // Prioritize horizontal movement over vertical movement
            if (movementInput.x > 0)
            {
                currentDirection = Direction.Right;

                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (movementInput.x < 0)
            {
                currentDirection = Direction.Left;

                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (movementInput.y > 0)
            {
                currentDirection = Direction.Up;
            }
            else if (movementInput.y < 0)
            {
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
    }


}
