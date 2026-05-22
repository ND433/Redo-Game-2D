using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 10f;
    public float tickRate = 1.0f;

    private float nextDamageTime;


    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleTickDamage(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTickDamage(other.gameObject);
    }
    private void HandleTickDamage(GameObject potentialPlayer)
    {
        if (Time.time >= nextDamageTime)
        {
            PlayerHealth player = potentialPlayer.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damageAmount);

                nextDamageTime = Time.time + tickRate;
            }
        }
    }
}