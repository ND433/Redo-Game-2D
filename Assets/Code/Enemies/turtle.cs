using UnityEngine;

public class EnemyShellMode : MonoBehaviour
{
    [Header("Shell Mode Settings")]
    // The fast speed when it gets kicked/angry at 1 HP
    public float shellSpeed = 12f;
    // Name of your Animator boolean parameter
    public string shellAnimationName = "IsShell";

    private EnemyHealth healthScript;
    private EnemyWalk walkScript;
    private Animator animator;
    private Collider2D myCollider;

    private bool isInShellMode = false;
    private float immunityTimer = 0.1f;
    private float lifeTimer = 10000f;

    // Keeps track of the player collider so we can reset collisions later
    private Collider2D playerCollider;

    void Start()
    {
        healthScript = GetComponent<EnemyHealth>();
        walkScript = GetComponent<EnemyWalk>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (healthScript != null && healthScript.health <= 1f && !isInShellMode)
        {
            TriggerShellMode();
        }

        if (isInShellMode)
        {
            if (immunityTimer > 0f)
            {
                immunityTimer -= Time.deltaTime;

                // When 1 second finishes, start colliding with the player again!
                if (immunityTimer <= 0f && myCollider != null && playerCollider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, playerCollider, false);
                }
            }

            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                if (healthScript != null)
                {
                    healthScript.TakeDamage(1f);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void TriggerShellMode()
    {
        isInShellMode = true;
        immunityTimer = 1f;
        lifeTimer = 10f;

        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();

            // CRITICAL FIX: Tell Unity physics to ignore JUST the player for 1 second.
            // The shell stays solid for floors and walls, but passes harmlessly through the player!
            if (myCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider, true);
            }
        }

        if (walkScript != null)
        {
            walkScript.moveSpeed = shellSpeed;
        }

        // Size multiplier line has been removed completely!

        if (animator != null)
        {
            animator.SetBool(shellAnimationName, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInShellMode && immunityTimer <= 0f)
        {
            EnemyHealth otherEnemy = collision.gameObject.GetComponent<EnemyHealth>();

            // Instantly kill other enemies it crashes into
            if (otherEnemy != null && otherEnemy != healthScript)
            {
                otherEnemy.TakeDamage(1f);
            }
        }
    }
}