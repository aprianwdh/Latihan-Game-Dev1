using UnityEngine;
using System.Collections;
public class Player : MonoBehaviour
{
    //public variable
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;

    //private variable
    private Rigidbody2D rb;
    bool isGrounded = false;
    SpriteRenderer spriteRenderer;

    //fungsi yang dijalankan sekali saat game dimulai    
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Jump();
        CekisGrounded();
    }

    //fungsi yang dijalankan setiap frame
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // fungsi untuk menggerakkan player
    private void MovePlayer()
    {
        float InputX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(InputX * speed, rb.linearVelocity.y);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, groundLayer);
        if (hit.collider != null)
        {
            isGrounded =  true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void TaakeDamage()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        gameManager.current_health--;
        LeanTween.color(gameObject, Color.red, 0.2f)
            .setEase(LeanTweenType.easeInOutQuint)
            .setOnComplete(() => LeanTween.color(gameObject,Color.white,0.2f));

    }
}
