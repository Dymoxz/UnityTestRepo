using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpAmount;
    [SerializeField] private float speed;
    [SerializeField] private Transform graphicsTransform; // Assign the "Graphics" child here
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.1f;


    [Header("Ability Settings")]
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float jumpMultiplier = 1.2f;
    [SerializeField] private float gravityMultiplier = 0.5f; // Lower gravity for "floaty" jumps
    [SerializeField] private Sprite abilitySprite;
    [SerializeField] private BoxCollider2D normalCollider;
    [SerializeField] private BoxCollider2D abilityCollider;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private float originalSpeed;
    private float originalGravity;
    private bool isAbilityActive = false;

    private bool isDashing;
    private float dashTimer;
    private float dashDirection;
    private float dashesRemaining = 2;

    public Canvas gameOverScreen;

    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private bool isGrounded;

    public Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();

        spriteRenderer = graphicsTransform.GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        originalSpeed = speed;
        originalGravity = rb.gravityScale;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Ability.performed += OnAbility;
    }

    void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Dash.performed -= OnDash;
        inputActions.Player.Ability.performed -= OnAbility;
        inputActions.Player.Disable();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (dashesRemaining > 0 && !isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
            //usedDash == 0 or 1 allows dash, if usedDash is 2 or more, it resets to 1 and allows dash again
            dashesRemaining--;
            dashDirection = moveInput.x != 0 ? Mathf.Sign(moveInput.x) : 1f;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse + 2);
    }

    private void OnAbility(InputAction.CallbackContext context)
    {
        if (!isAbilityActive)
        {
            StartCoroutine(ActivateAbility());
        }
    }
    private IEnumerator ActivateAbility()
    {
        isAbilityActive = true;
        animator.SetInteger("FormType", 1); // Switch to Ability animations

        // Apply Buffs
        speed *= speedMultiplier;
        jumpAmount *= jumpMultiplier;
        rb.gravityScale *= gravityMultiplier;

        normalCollider.enabled = false;
        abilityCollider.enabled = true;

        // Wait for the duration
        yield return new WaitForSeconds(abilityDuration);

        // Reset Values
        animator.SetInteger("FormType", 0); // Switch back to Normal        speed /= speedMultiplier;
        jumpAmount /= jumpMultiplier;
        rb.gravityScale = originalGravity;
        speed = originalSpeed;

        normalCollider.enabled = true;
        abilityCollider.enabled = false;

        isAbilityActive = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
       
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

            if (dashTimer <= 0f)
                isDashing = false;

            return; 
        }
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));

        Flip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
            dashesRemaining = 2;

        if (collision.gameObject.CompareTag("Game Over"))
        {
            gameOverScreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    private void Flip()
    {
        if (moveInput.x > 0)
        {
            graphicsTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            graphicsTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}