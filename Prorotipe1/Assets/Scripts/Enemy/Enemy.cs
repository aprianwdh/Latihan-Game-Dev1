using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    public float speed = 5.0f;
    public float chaseRange = 10.0f;
    public Transform Guardhouse;


    private int currentHealth;
    private Rigidbody2D rb;
    private bool isChase = false;
    private Transform Targer;
    private Animator anim;
    private bool isWalk = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        Targer = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimationControler();
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        //jika player berada dalam jarak chase
        if (Vector2.Distance(transform.position, Targer.position) < chaseRange)
        {
            isChase = true;
            isWalk = true;
            if (Targer.position.x > transform.position.x)
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
                Debug.Log("Sudah sampai di Guardhouse");
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
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
