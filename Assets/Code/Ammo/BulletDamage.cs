using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    //damage amt
    public float damageAmount = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            //check enemy health we dont want instant death
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                //do damage to the enemy
                enemy.TakeDamage(damageAmount);
            }
            //murder the bullet
            Destroy(gameObject);
        }
        //if wall just murder bullet instand not used but in case
        if (other.CompareTag("Wall"))
        {
            //murder
            Destroy(gameObject);
        }
    }
}