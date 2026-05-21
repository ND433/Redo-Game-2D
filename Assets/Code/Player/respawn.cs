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
    private bool isImmune = false;
    private float immunityTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerHealthScript == null) playerHealthScript = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isImmune)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0f) isImmune = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 1. KILL MODE: Destroy enemy on touch
            if (isKillMode)
            {
                var enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null) enemy.TakeDamage(999f);
                else Destroy(collision.gameObject);
                return;
            }

            // 2. INVINCIBLE: Ignore damage
            if (isInvincible) return;

            // 3. NORMAL STOMP/DAMAGE LOGIC
            bool isAboveEnemy = transform.position.y > (collision.transform.position.y + 0.2f);
            if (isAboveEnemy)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                var enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null) enemy.TakeDamage(1f);
                isImmune = true;
                immunityTimer = 0.3f;
            }
            else if (!isImmune)
            {
                if (playerHealthScript != null) playerHealthScript.TakeDamage(1f);
                isImmune = true;
                immunityTimer = 1.5f;
            }
        }
    }
}