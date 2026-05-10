using UnityEngine;

public class CauldronEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private bool shouldMove = false;
    private Animator anim;
    private Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // This is called by the Trigger Zone
    public void StartRunning()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Physics turns "on"
        shouldMove = true;
        anim.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        if (shouldMove)
        {
            // Move forward continuously
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Died!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpPad"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 4f);
        }
    }
}