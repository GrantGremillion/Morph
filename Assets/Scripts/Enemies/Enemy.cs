using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public bool debug = false;
    
    // Object references
    [HideInInspector] public new Rigidbody2D rigidbody;
    public HealthBar healthBar;
    public Arrow arrow;
    public ThrowingStar throwingStar;
    public Bananarang bananarang;

    [HideInInspector] public Vector2 targetDirection;
    [HideInInspector] public Vector2 currentDirection = Vector2.zero;
    public State currentState;
    public Canvas healthBarCanvas;
    public float speed;
    public float health;
    public float maxHealth;
    public float immunityTime = 0.5f;
    public bool pauseAnimation;
    public string itemDropType;
    public int numberOfDrops;

    public Animator animator;

    [HideInInspector] public bool canAttack;     ///
    public bool isAttacking;
    private bool previousAwareOfPlayer = false;
    public bool awareOfPlayer = false;

    protected SpriteRenderer spriteRenderer;
    public Color flashColor; 
    public float flashDuration = 1.0f; 
    private Color originalColor;

    public enum State
    {
        Left,
        Right,
        Hurt,
        Dead,
        Agro,
        Deagro,
        Idle,
        Attack,
        Up,
        Down,
        None
    }

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        healthBar = healthBarCanvas.GetComponent<HealthBar>();

        healthBar.SetMaxHealth(health);
        healthBar.gameObject.SetActive(false);

        pauseAnimation = false;
        canAttack = false;
        previousAwareOfPlayer = awareOfPlayer;  

        originalColor = spriteRenderer.color; 
    }

    void Update()
    {
        UpdateState();
    }

    public virtual void UpdateState() { }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (collision != null)
            {
                StartCoroutine(TakeDamage(collision,"Arrow"));
            }
        }
        if (collision.gameObject.CompareTag("ThrowingStar"))
        {
            if (collision != null)
            {
                StartCoroutine(TakeDamage(collision, "ThrowingStar"));
            }
        }
        if (collision.gameObject.CompareTag("Bananarang"))
        {
            if (collision != null)
            {
                StartCoroutine(TakeDamage(collision, "Bananarang"));
            }
        }
    }

    public IEnumerator TakeDamage(Collision2D collision, string name)
    {
 
        StartCoroutine(Flash());
        
        if (name == "Arrow")
        {
            arrow = collision.gameObject.GetComponent<Arrow>();
            healthBar.TakeDamage(arrow.damage);
        }
        else if (name == "ThrowingStar")
        {
            throwingStar = collision.gameObject.GetComponent<ThrowingStar>();
            healthBar.TakeDamage(throwingStar.damage);
        }
        else if (name == "Bananarang")
        {
            bananarang = collision.gameObject.GetComponent<Bananarang>();
            healthBar.TakeDamage(bananarang.damage);
        }

        //print("Current health:" + health + "take damage: " + arrow.damage);

        if (healthBar.GetDesiredHealth() <= 0)
        {
            speed = 0;
            gameObject.GetComponent<Collider2D> ().enabled = false;
            animator.SetBool("Dead",true);
            healthBar.gameObject.SetActive(false);
            DropItems();
            yield return new WaitForSeconds(immunityTime);
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(immunityTime);
    }

    private IEnumerator Flash()
    {
        if (spriteRenderer == null) print("null");
        spriteRenderer.color = flashColor; 
        yield return new WaitForSeconds(flashDuration); 
        spriteRenderer.color = originalColor; 
    }

    public void DropItems()
    {
        for (int i = 0; i < numberOfDrops; i++)
        {
            Vector3 spawnPosition = ItemSpawner.Instance.GetRandomSpawnPosition(transform.position);
            ItemSpawner.Instance.SpawnEnemyDrops(itemDropType, spawnPosition);
        }
    }
}
