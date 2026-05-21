using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement Settings")]
    //enemy speed
    public float speed = 3f;
    //enemy jump force
    public float jumpForce = 6f;

    [Header("Detection Settings")]
    //enemy detect player range
    public float detectionRange = 5f;
    //idk why its called this but just red alr
    public Color gizmoColor = Color.red;

    //get regidbody
    private Rigidbody2D rb;
    //make sure enemy faces the right way when moving
    private float originalScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScaleX = Mathf.Abs(transform.localScale.x);
    }
    //move enemy towards player
    public void Move(Vector2 target, float currentSpeed)
    {
        float dirX = target.x - transform.position.x;
        // If the enemy has a RB and is not god use physics in movement
        if (rb != null && rb.bodyType != RigidbodyType2D.Static)
        {
            if (rb.gravityScale > 0) 
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(dirX) * currentSpeed, rb.linearVelocity.y);

                if (target.y > transform.position.y + 1f && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            // If gravity is zero move towards the player without physics
            else
            {
                Vector2 moveDir = (target - (Vector2)transform.position).normalized;
                rb.linearVelocity = moveDir * currentSpeed;
            }
        }
        else 
        {
            transform.position = Vector2.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        }
        // Flip the enemy to face the player
        if (Mathf.Abs(dirX) > 0.1f)
        {
            float face = Mathf.Sign(dirX) * originalScaleX;
            transform.localScale = new Vector3(face, transform.localScale.y, transform.localScale.z);
        }
    }
    public void Move(Vector2 target)
    {
        Move(target, speed);
    }
    //show circle around enemy to show its detection range
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}