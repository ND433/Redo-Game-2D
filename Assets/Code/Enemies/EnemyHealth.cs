using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 30f;

    [Header("Death Assets (Set per Prefab)")]
    public AudioClip deathSound;
    [Range(0f, 1f)] public float soundVolume = 1f;
    public string deathAnimationName = "IsDead";
    public float destructionDelay = 0.5f;

    private bool isDead = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        //if (ScoreManager.instance != null)
        //{
        //    //show dead and alive ones text at start
        //    ScoreManager.remainingEnemies = ScoreManager.remainingEnemies + 1;
        //    ScoreManager.instance.UpdateUI();
        //}
    }

    public void TakeDamage(float amount)
    {
        health = health - amount;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        //ScoreManager.killedEnemies = ScoreManager.killedEnemies + 1;
        //ScoreManager.remainingEnemies = ScoreManager.remainingEnemies - 1;
        //ScoreManager.instance.UpdateUI();

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, soundVolume);
        }

        if (anim != null && !string.IsNullOrEmpty(deathAnimationName))
        {
            anim.SetBool(deathAnimationName, true);
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        MonoBehaviour walkComponent = GetComponent("EnemyWalk") as MonoBehaviour;
        if (walkComponent != null) walkComponent.enabled = false;

        Destroy(gameObject, destructionDelay);
    }
}