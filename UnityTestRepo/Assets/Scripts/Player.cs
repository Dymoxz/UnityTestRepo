using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Rigidbody2D rb;
    public float jumpAmount;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float speed;
    private bool isGrounded;
    public Canvas gameOverScreen;

    bool isCollidingWithDamage;

    void Update()
    {
        if (isCollidingWithDamage)
        {
            gameOverScreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        }
        float moveInput = Input.GetAxisRaw("Horizontal"); // -1, 0 or 1
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            isCollidingWithDamage = true;
           Debug.Log("Collided with damage object!");
        }
    }
}
