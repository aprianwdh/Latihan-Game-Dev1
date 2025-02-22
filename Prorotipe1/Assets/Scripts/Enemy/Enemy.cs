using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    public float speed = 5.0f;
    public float chaseRange = 10.0f;
    public float attackRange = 1.0f;
    public Transform Guardhouse;
    public int damage = 1;
    public LayerMask playerLayer;
    public GameObject BloodEffect;


    private int currentHealth;
    private Rigidbody2D rb;
    private bool isChase = false;
    private Transform Target;
    private Animator anim;
    private bool isWalk = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimationControler();
        PlayerInRange();
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        //jika player berada dalam jarak chase
        if (Vector2.Distance(transform.position, Target.position) < chaseRange)
        {
            isChase = true;
            isWalk = true;
            if (Target.position.x > transform.position.x)
            {
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        //jika player diluar jangkauan chase
        else
        {
            isChase = false;
            backToGuardHouse();
        }
    }

    void backToGuardHouse()
    {
        if (!isChase)
        {
            float distanceToGuardhouse = Vector2.Distance(transform.position, Guardhouse.position);

            // Jika belum sampai di Guardhouse
            if (distanceToGuardhouse > 0.1f)
            {
                Vector2 directionToGuardhouse = (Guardhouse.position - transform.position).normalized;
                rb.linearVelocity = directionToGuardhouse * speed;

                // Menyesuaikan arah musuh
                if (Guardhouse.position.x > transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                isWalk = true;
            }
            else
            {
                // Jika sudah sampai, berhenti
                rb.linearVelocity = Vector2.zero;
                isWalk = false;
            }
        }
    }

    void PlayerInRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hit != null)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                anim.SetTrigger("Attack");
            }
        }
    }
    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hit != null)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    void AnimationControler()
    {
        anim.SetBool("isChase", isWalk);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        LeanTween.color(gameObject, Color.red, 0.2f)
        .setEase(LeanTweenType.easeInOutQuint)
        .setOnComplete(() =>
        {
            LeanTween.color(gameObject, Color.white, 0.2f)
            .setEase(LeanTweenType.easeInOutQuint)
            .setOnComplete(() =>
            {
                if (currentHealth <= 0)
                {
                    Die();
                }
            });
        });

    }

    void Die()
    {
        anim.SetBool("isDie", true);
        Instantiate(BloodEffect, transform.position, Quaternion.identity);
        Destroy(gameObject,0.5f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
