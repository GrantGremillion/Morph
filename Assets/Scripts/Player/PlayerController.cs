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
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }


    [SerializeField] private float speed;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = movementInput * speed;
    }

    void Update()
    {
        if (isDashing)
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
            if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y))
            {
                if (movementInput.x > 0)
                {
                    currentDirection = Direction.Right;
                }
                else
                {
                    currentDirection = Direction.Left;
                }
            }
            else
            {
                if (movementInput.y > 0)
                {
                    currentDirection = Direction.Up;
                }
                else
                {
                    currentDirection = Direction.Down;
                }
            }

            // Optional: Log the current direction for debugging purposes
            Debug.Log("Current Direction: " + currentDirection);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (currentDirection == Direction.Left)
        {
            rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        }
        else if (currentDirection == Direction.Right)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else if (currentDirection == Direction.Up)
        {
            rb.velocity = new Vector2(0f,transform.localScale.y * dashingPower);
        }
        else
        {
            rb.velocity = new Vector2(0f,-transform.localScale.y * dashingPower);
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
}
