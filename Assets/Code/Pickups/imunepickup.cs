using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public float duration = 60f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Automatically find the PlayerCollision script on the player
        if (other.CompareTag("Player"))
        {
            PlayerCollision pc = other.GetComponent<PlayerCollision>();
            if (pc != null)
            {
                StartCoroutine(ActivatePowerUp(pc));
            }
        }
    }

    IEnumerator ActivatePowerUp(PlayerCollision pc)
    {
        // Turn on power-up effects
        pc.isInvincible = true;
        pc.isKillMode = true;

        // Hide the power-up so it can't be picked up again
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Collider2D col = GetComponent<Collider2D>();
        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;

        // Wait for 60 seconds
        yield return new WaitForSeconds(duration);

        // Turn off power-up effects
        pc.isInvincible = false;
        pc.isKillMode = false;

        Destroy(gameObject);
    }
}