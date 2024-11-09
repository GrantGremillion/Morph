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

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        healthBar = healthBarCanvas.GetComponent<HealthBar>();

        healthBar.SetMaxHealth(health);
        healthBar.gameObject.SetActive(false);

        pauseAnimation = false;
        canAttack = false;
        previousAwareOfPlayer = awareOfPlayer;  
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
            animator.SetBool("Damaged",true);
            StartCoroutine(TakeDamage(collision,"Arrow"));
        }
        if (collision.gameObject.CompareTag("ThrowingStar"))
        {
            animator.SetBool("Damaged",true);
            StartCoroutine(TakeDamage(collision,"ThrowingStar"));
        }
    }

    public IEnumerator TakeDamage(Collision2D collision, string name)
    {
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

        //print("Current health:" + health + "take damage: " + arrow.damage);

        if (healthBar.GetDesiredHealth() <= 0)
        {
            animator.SetBool("Damaged",false);
            animator.SetBool("Dead",true);
            healthBar.gameObject.SetActive(false);
            DropItems();
            yield return new WaitForSeconds(immunityTime);
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(immunityTime);
        animator.SetBool("Damaged",false);

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
