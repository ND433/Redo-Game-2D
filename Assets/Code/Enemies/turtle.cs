using UnityEngine;

public class EnemyShellMode : MonoBehaviour
{
    [Header("Shell Mode Settings")]
    public float shellSpeed = 12f;
    public string shellAnimationName = "IsShell";
    public string rollAnimationName = "IsRoll";

    private EnemyHealth healthScript;
    private EnemyWalk walkScript;
    private Animator animator;
    private Collider2D myCollider;

    private bool isInShellMode = false;
    private float immunityTimer = 0.1f;
    private float lifeTimer = 10000f;

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

                if (immunityTimer <= 0f && myCollider != null && playerCollider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, playerCollider, false);
                }
            }

            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                if (animator != null)
                {
                    animator.SetBool(rollAnimationName, false);
                }

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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();

            if (myCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider, true);
            }
        }

        if (walkScript != null)
        {
            walkScript.moveSpeed = shellSpeed;
        }

        if (animator != null)
        {
            animator.SetBool(shellAnimationName, true);
            animator.SetBool(rollAnimationName, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInShellMode && immunityTimer <= 0f)
        {
            EnemyHealth otherEnemy = collision.gameObject.GetComponent<EnemyHealth>();

            if (otherEnemy != null && otherEnemy != healthScript)
            {
                otherEnemy.TakeDamage(1f);
            }
        }
    }
}