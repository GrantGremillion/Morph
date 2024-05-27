
using UnityEngine;

public class MushroomEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        dropType = "banana";
        maxHealth = 10;
        speed = 0.3f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if (playerAwarenessController.awareOfPlayer)
        {
            targetDirection = playerAwarenessController.directionToPlayer;
            currentState = State.Left;
        }
        else
        {
            targetDirection = Vector2.zero;
            currentState = State.Idle;
        }
    }

 
    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rigidbody.velocity = Vector2.zero;
        }
        else
        {
            rigidbody.velocity = targetDirection * speed;
        }
    }

}
