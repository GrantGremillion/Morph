using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public Animator animator;
    [SerializeField] private float animationSpeed;
    public GameObject arrow;
    public GameObject spawn;
    private Transform playerTransform;
    private float distanceFromPlayer = 0.08f; // Distance of the bow from the player

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform.parent;
        animator.speed = animationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Turning();
    }

    void Turning()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Calculate the direction from the player to the mouse
        Vector2 direction = new Vector2(mousePos.x - playerTransform.position.x, mousePos.y - playerTransform.position.y);

        // Calculate the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate the new position for the bow
        Vector3 bowPosition = playerTransform.position + (Vector3)direction.normalized * distanceFromPlayer;

        // Set the bow's position and rotation
        transform.position = bowPosition;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
