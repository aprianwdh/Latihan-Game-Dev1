using UnityEngine;

public class Player : MonoBehaviour
{
    //public variable
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;

    //private variable
    private Rigidbody2D rb;
    bool isGrounded = false;

    //fungsi yang dijalankan sekali saat game dimulai    
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
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
        Debug.Log(isGrounded);
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

}
