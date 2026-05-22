using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float damageAmount = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}