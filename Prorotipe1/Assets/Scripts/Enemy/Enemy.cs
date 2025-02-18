using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    public float speed = 5.0f;
    public float chaseRange = 10.0f;


    private int currentHealth;
    private Rigidbody2D rb;
    private bool isChase = false;
    private Transform Targer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        Targer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        if (Vector2.Distance(transform.position, Targer.position) < chaseRange)
        {
            isChase = true;
            Debug.Log("Chase");

            if (Targer.position.x > transform.position.x)
            {
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            isChase = false;
            Debug.Log("Not Chase");
        }
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
