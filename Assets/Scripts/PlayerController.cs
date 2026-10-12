using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves the player left/right and handles jumping using actions from an Input Actions asset.
/// Movement is physics-based through Rigidbody2D. The player can only jump while standing
/// on something tagged "Ground".
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float fastFallSpeed = 15f;

    [Header("Visuals")]
    [Tooltip("Tick this if the original sprite art faces right. Leave it unticked if it faces left.")]
    [SerializeField] private bool spriteFacesRight = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        // Ignore input while the game is paused or the win screen is up.
        if (Time.timeScale == 0f)
        {
            return;
        }

        // Input is checked in Update so a quick key press is never missed.
        if (jumpAction.action.WasPressedThisFrame() && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        UpdateFacing();
    }

    private void FixedUpdate()
    {
        // Physics movement belongs in FixedUpdate.
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        float yVelocity = rb.linearVelocity.y;

        // Holding Down in mid-air drops the player quickly.
        if (!isGrounded && moveInput.y < 0f)
        {
            yVelocity = Mathf.Min(yVelocity, -fastFallSpeed);
        }

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, yVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGrounded(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGrounded(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// Mirrors the sprite to face the direction the player is moving.
    /// Flipping the sprite (instead of rotating the object) leaves the collider and physics untouched.
    /// </summary>
    private void UpdateFacing()
    {
        float moveX = moveAction.action.ReadValue<Vector2>().x;
        if (moveX > 0f)
        {
            spriteRenderer.flipX = !spriteFacesRight;
        }
        else if (moveX < 0f)
        {
            spriteRenderer.flipX = spriteFacesRight;
        }
        // When no key is held, keep facing the last direction.
    }

    /// <summary>
    /// The player is grounded only when touching Ground from above, so touching a wall
    /// does not allow wall-jumping.
    /// </summary>
    private void UpdateGrounded(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            return;
        }

        bool touchingFloor = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                touchingFloor = true;
                break;
            }
        }
        isGrounded = touchingFloor;
    }
}