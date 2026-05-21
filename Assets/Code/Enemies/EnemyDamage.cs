using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    //enemy danagous or not i ques damage amount and tick rate aka how much damage per sec
    public float damageAmount = 10f;
    public float tickRate = 1.0f;

    //makes sure enemy isnt a machine gun
    private float nextDamageTime;


    //if player still at enemy dont hit again
    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleTickDamage(collision.gameObject);
    }
    //just same but for trigger cuz yea spikes
    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTickDamage(other.gameObject);
    }
    //does pain
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