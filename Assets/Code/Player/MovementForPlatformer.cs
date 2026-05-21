using UnityEngine;

public class MovementForPlatformer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float plMaxSpeed = 7f;
    public float jumpForce = 15f;
    public float gravityScale = 4f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    public Color editorGroundRayColor = Color.cyan;

    [Header("Magnet Circle")]
    public float magnetRange = 5f;
    public Color editorCircleColor = Color.yellow;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // REMOVED: initialScale = transform.localScale; -> This was locking your size!

        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (col == null) col = GetComponent<Collider2D>();

        Vector2 rayStart = new Vector2(col.bounds.center.x, col.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        HandleMagnetLogic();
        HandleMovement();
    }

    void HandleMagnetLogic()
    {
        Collider2D[] itemsInRange = Physics2D.OverlapCircleAll(transform.position, magnetRange);
        foreach (Collider2D item in itemsInRange)
        {
            if (item.CompareTag("Drag"))
            {
                PistolAmmoPickup ammo = item.GetComponent<PistolAmmoPickup>();
                if (ammo != null) ammo.isDragging = true;
            }
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * plMaxSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeOff");
        }

        anim.SetBool("isRunning", moveInput != 0);
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("isAttacking", Input.GetMouseButton(0));

        // FIXED FLIP LOGIC:
        // Reads your current size (whether PlayerSizeManager made you tiny or big) 
        // and only modifies the positive/negative direction (+ or -) on the X axis.
        if (moveInput != 0)
        {
            float currentXScale = Mathf.Abs(transform.localScale.x); // Gets the absolute size thickness
            float currentYScale = transform.localScale.y;           // Gets current tiny or big height
            float currentZScale = transform.localScale.z;

            // Apply direction based on moveInput without altering the scale values
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * currentXScale, currentYScale, currentZScale);
        }
    }

    private void OnDrawGizmos()
    {
        Collider2D gizmoCol = GetComponent<Collider2D>();
        if (gizmoCol != null)
        {
            Gizmos.color = editorGroundRayColor;
            Vector3 rayStart = new Vector3(gizmoCol.bounds.center.x, gizmoCol.bounds.min.y, 0);
            Vector3 rayEnd = rayStart + (Vector3.down * groundCheckDistance);
            Gizmos.DrawLine(rayStart, rayEnd);
        }

        Gizmos.color = editorCircleColor;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}