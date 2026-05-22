using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public float jumpForce = 6f;

    [Header("Detection Settings")]
    public float detectionRange = 5f;
    public Color gizmoColor = Color.red;

    private Rigidbody2D rb;
    private float originalScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScaleX = Mathf.Abs(transform.localScale.x);
    }
    public void Move(Vector2 target, float currentSpeed)
    {
        float dirX = target.x - transform.position.x;
        if (rb != null && rb.bodyType != RigidbodyType2D.Static)
        {
            if (rb.gravityScale > 0) 
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(dirX) * currentSpeed, rb.linearVelocity.y);

                if (target.y > transform.position.y + 1f && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}