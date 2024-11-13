using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bananarang : Weapon
{
    public float throwingSpeed;
    public float returnSpeed;
    public bool beingThrown;
    public bool returningToPlayer = false;
    public float distanceFromPlayer = 0.9f; // Distance of the projectile from the player
    public float damage = 5;

    public float returnTime;

    public bool rotate = false;
    public float rotationSpeed = 3f;

    private WeaponManager weaponManager;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        type = "Bananarang";
        beingThrown = false;
        player = FindAnyObjectByType<PlayerController>();
        playerTransform = player.transform;
        upgradeSystem = player.GetComponentInChildren<UpgradeSystem>();
        weaponManager = FindObjectOfType<WeaponManager>();
        rb = GetComponent<Rigidbody2D>();
        returnTime = 1;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = GetComponent<Collider2D>();

        BananarangRotation();
        if (beingThrown)
        {
            collider.enabled = true; 
        }
        else collider.enabled = false;
    }

    void BananarangRotation()
    {
        if (beingThrown) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(mousePos.x - playerTransform.position.x, mousePos.y - playerTransform.position.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 StarPosition = playerTransform.position + (Vector3)direction.normalized * distanceFromPlayer;

        transform.position = StarPosition;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        returningToPlayer = true;
        returnTime = 1.0f;
    }

}
