using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    [Header("Movement Settings")]
    //speed of the patrol
    public float moveSpeed = 3f;
    private bool movingRight = true;

    [Header("Detection Settings")]
    //how far down the ray goes to find floor
    public float detectionDistance = 0.5f;
    //how far forward to check for walls/prefabs
    public float wallCheckDistance = 0.5f;
    //offset to start the ray from the front of character
    public float frontOffset = 0.5f;
    //layer to check for collisions (Tilemap and Prefabs)
    public LayerMask detectionLayer;

    [Header("Editor Visuals")]
    //color of the raycast in editor
    public Color rayColor = Color.red;

    private Rigidbody2D rb;
    private Vector3 initialScale;

    void Start()
    {
        //get rigidbody
        rb = GetComponent<Rigidbody2D>();
        //save original scale to prevent size changing
        initialScale = transform.localScale;

        //freeze rotation so character stays upright
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        HandlePatrol();
    }

    void HandlePatrol()
    {
        //determine direction
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        //apply movement
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        //set ray start point to the front of the character
        Vector2 rayStart = (Vector2)transform.position + (direction * frontOffset);

        // 1. Check for floor (angled down)
        RaycastHit2D groundInfo = Physics2D.Raycast(rayStart, Vector2.down + direction, detectionDistance, detectionLayer);

        // 2. Check for wall/prefab (straight ahead)
        RaycastHit2D wallInfo = Physics2D.Raycast(rayStart, direction, wallCheckDistance, detectionLayer);

        // FLIP IF: No ground is hit OR a wall/prefab is hit
        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        //flip scale while keeping original inspector size
        float newXScale = movingRight ? initialScale.x : -initialScale.x;
        transform.localScale = new Vector3(newXScale, initialScale.y, initialScale.z);
    }

    //draw the detection lines in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Vector3 rayStart = transform.position + (Vector3)(direction * frontOffset);

        // Draw Floor Check
        Vector3 groundEnd = rayStart + (Vector3)(Vector2.down + direction).normalized * detectionDistance;
        Gizmos.DrawLine(rayStart, groundEnd);

        // Draw Wall Check
        Vector3 wallEnd = rayStart + (Vector3)direction * wallCheckDistance;
        Gizmos.DrawLine(rayStart, wallEnd);
    }
}