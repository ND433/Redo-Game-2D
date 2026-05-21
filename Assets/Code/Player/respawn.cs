using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerHealth playerHealthScript;
    public float bounceForce = 12f;

    [Header("PowerUp Status")]
    public bool isInvincible = false;
    public bool isKillMode = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerHealthScript == null) playerHealthScript = GetComponent<PlayerHealth>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isKillMode)
            {
                var enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null) enemy.TakeDamage(999f);
                else Destroy(collision.gameObject);
                return;
            }

            float dotProduct = Vector2.Dot(collision.GetContact(0).normal, Vector2.up);
            bool isStomping = dotProduct > 0.5f;

            if (isStomping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                var enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null) enemy.TakeDamage(1f);
            }
            else if (!isInvincible && playerHealthScript.health < 2f)
            {
                // Simple call to take damage. Death logic is now inside PlayerHealth!
                playerHealthScript.TakeDamage(1f);
            }
        }
    }
}