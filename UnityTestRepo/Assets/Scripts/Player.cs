using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpAmount;
    [SerializeField] private float speed;
    [SerializeField] private float gravity = -20f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public Canvas gameOverScreen;

    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isGrounded;
    private bool isCollidingWithDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpAmount * -2f * gravity);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded)
            verticalVelocity += gravity * Time.deltaTime;
        else if (verticalVelocity < 0)
            verticalVelocity = -2f;

        rb.linearVelocity = new Vector2(moveInput.x * speed, verticalVelocity);

        if (isCollidingWithDamage)
        {
            gameOverScreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game Over"))
        {
            isCollidingWithDamage = true;
            Debug.Log("Collided with damage object!");
        }
    }
}