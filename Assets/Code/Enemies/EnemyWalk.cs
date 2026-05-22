using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    private bool movingRight = true;

    [Header("Detection Settings")]
    public float detectionDistance = 0.5f;
    public float wallCheckDistance = 0.5f;
    public float frontOffset = 0.5f;
    public LayerMask detectionLayer;

    [Header("Editor Visuals")]
    public Color rayColor = Color.red;

    private Rigidbody2D rb;
    private Vector3 initialScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        HandlePatrol();
    }

    void HandlePatrol()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        Vector2 rayStart = (Vector2)transform.position + (direction * frontOffset);

        RaycastHit2D groundInfo = Physics2D.Raycast(rayStart, Vector2.down + direction, detectionDistance, detectionLayer);

        RaycastHit2D wallInfo = Physics2D.Raycast(rayStart, direction, wallCheckDistance, detectionLayer);

        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        float newXScale = movingRight ? initialScale.x : -initialScale.x;
        transform.localScale = new Vector3(newXScale, initialScale.y, initialScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Vector3 rayStart = transform.position + (Vector3)(direction * frontOffset);

        Vector3 groundEnd = rayStart + (Vector3)(Vector2.down + direction).normalized * detectionDistance;
        Gizmos.DrawLine(rayStart, groundEnd);

        Vector3 wallEnd = rayStart + (Vector3)direction * wallCheckDistance;
        Gizmos.DrawLine(rayStart, wallEnd);
    }
}