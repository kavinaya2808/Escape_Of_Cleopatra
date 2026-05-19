using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 3f;
    public float jumpingPower = 6f;
    private bool isFacingRight = true;
    public bool hasKey = false; // ✅ Track key possession

    [SerializeField] private Rigidbody2D rb;
    public Rigidbody2D Rigidbody => rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private SpriteRenderer spriteRenderer;

    // Normal Sprites
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite moveSprite1;
    [SerializeField] private Sprite moveSprite2;
    [SerializeField] private Sprite jumpSprite;

    // Sprites With Key
    [SerializeField] private Sprite idleWithKeySprite;
    [SerializeField] private Sprite moveWithKeySprite1;
    [SerializeField] private Sprite moveWithKeySprite2;
    [SerializeField] private Sprite jumpWithKeySprite;

    public PlayerSFX playerSFX;

    private float walkAnimTimer = 0f;
    private float walkAnimInterval = 0.2f;
    private bool useFirstStep = true;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump input
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            if (playerSFX != null)
                playerSFX.PlayJump();
        }

        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.UpArrow)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();
        SwapSprite();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void SwapSprite()
    {
        bool isJumping = !IsGrounded();
        bool isWalking = Mathf.Abs(horizontal) > 0.01f;

        // 🔀 Choose the correct sprite set
        Sprite idle = hasKey ? idleWithKeySprite : idleSprite;
        Sprite jump = hasKey ? jumpWithKeySprite : jumpSprite;
        Sprite walk1 = hasKey ? moveWithKeySprite1 : moveSprite1;
        Sprite walk2 = hasKey ? moveWithKeySprite2 : moveSprite2;

        if (isJumping)
        {
            if (isWalking)
                spriteRenderer.sprite = jump;
            else
                spriteRenderer.sprite = idle; // Jumping straight up = idle sprite
        }
        else if (isWalking)
        {
            walkAnimTimer += Time.deltaTime;
            if (walkAnimTimer >= walkAnimInterval)
            {
                walkAnimTimer = 0f;
                useFirstStep = !useFirstStep;
            }
            spriteRenderer.sprite = useFirstStep ? walk1 : walk2;
        }
        else
        {
            spriteRenderer.sprite = idle;
        }
    }

    // 🟡 Call this method when player picks up the key
    public void PickupKey()
    {
        hasKey = true;
    }
}
