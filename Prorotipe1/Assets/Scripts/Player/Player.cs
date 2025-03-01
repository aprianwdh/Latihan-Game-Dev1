using UnityEngine;
using System.Collections;
public class Player : MonoBehaviour
{
    //public variable
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;
    public float groundCheckRadius = 1f;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public float dashTime = 0.1f;
    public float dashColldown = 1.0f;
    public float dashPower = 24f;
    public bool isDashing = false;
    public bool canDash = true;
    public TrailRenderer trailRenderer;
    public GameObject checkPoint;
    public GameObject BloodEffect;
    public GameObject interactIcon;


    //private variable
    private Rigidbody2D rb;
    bool isGrounded = false;
    SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool died = false;


    //fungsi yang dijalankan sekali saat game dimulai    
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        ifInteract();
        Dashing();
        Jump();
        CekisGrounded();
        Flip();
        AnimationController();
    }

    //fungsi yang dijalankan setiap frame
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        MovePlayer();
    }

    // fungsi untuk menggerakkan player
    private void MovePlayer()
    {
        float InputX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(InputX * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        //jika player bergerak ke kiri, maka player akan dibalikkan
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        //jika player bergerak ke kanan, maka player akan dibalikkan
        else if (Input.GetAxis("Horizontal") > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }


    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void CekisGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckRadius, groundLayer);
        if (hit.collider != null)
        {
            isGrounded =  true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void doAttack()//fungsi untuk menyerang dipanggil di animasi
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (died)
        {
            return;
        }
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        gameManager.current_health_player -= damageAmount;
        LeanTween.color(gameObject, Color.red, 0.2f)
            .setEase(LeanTweenType.easeInOutQuint)
            .setOnComplete(() => LeanTween.color(gameObject,Color.white,0.2f));
        Die();

    }

    void Die()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (gameManager.current_health_player <= 0)
        {
            died = true;
            Instantiate(BloodEffect, transform.position, Quaternion.identity);
            StartCoroutine(Respawn(0.5f));
            gameManager.current_health_player = 3;
            died = false;
        }
    }

    IEnumerator Respawn(float TimeRespawn)
    {
        yield return new WaitForSeconds(TimeRespawn);
        if (checkPoint != null)
        {
            transform.position = checkPoint.transform.position;
        }
        else
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void Dashing()
    {
        if ((Input.GetButtonDown("Fire2") || Input.GetMouseButtonDown(1))&& canDash == true)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float InputX = Input.GetAxis("Horizontal");

        canDash = false;
        isDashing = true;
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(InputX * dashPower, 0f);
        trailRenderer.emitting = true;

        // Ubah layer agar tidak bertabrakan dengan musuh
        gameObject.layer = LayerMask.NameToLayer("DashingPlayer");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DashingPlayer"), LayerMask.NameToLayer("Enemy"), true);

        yield return new WaitForSeconds(dashTime);

        // Kembalikan ke layer awal
        gameObject.layer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DashingPlayer"), LayerMask.NameToLayer("Enemy"), false);

        trailRenderer.emitting = false;
        isDashing = false;
        rb.gravityScale = OriginalGravity;

        yield return new WaitForSeconds(dashColldown);
        canDash = true;
    }

    void ifInteract()
    {
        if (FindAnyObjectByType<GameManager>().interactable)
        {
            interactIcon.SetActive(true);
        }
        else
        {
            interactIcon.SetActive(false);
        }
    }


    void AnimationController()
    {
        //Menatur animasi untuk berjala atau idle/////////////////////
        if (Input.GetAxis("Horizontal") != 0 && isGrounded == true)
        {
            //jika player bergerak, maka animasi player berjalan
            animator.SetBool("IsWalk", true);
        }
        else if (Input.GetAxis("Horizontal") == 0 && isGrounded == true)
        {
            //jika player tidak bergerak, maka animasi player idle
            animator.SetBool("IsWalk", false);
        }

        //menatur animasi untuk melompat/////////////////////////
        if (isGrounded == true)
        {
            //jika player melompat, maka animasi player melompat
            animator.SetBool("IsJump",false);
        }
        else if(isGrounded == false)
        {
            //jika player tidak melompat, maka animasi player idle / berjalan
            animator.SetBool("IsJump", true);
        }

        //menatur animasi untuk attack/////////////////////////
        if (Input.GetButtonDown("Fire1"))
        {
            //jika player menyerang, maka animasi player attack
            animator.SetTrigger("IsAttack");
        }
    }

    private void OnDrawGizmos()
    {
        //gizmo untuk chack ground
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckRadius);

        //gizmo untuk attack point
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
