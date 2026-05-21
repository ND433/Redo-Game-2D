using UnityEngine;
using System.Collections;

public class HealingPickup : MonoBehaviour
{
    //heal amount
    public float healAmount = 10f;
    //delay before pickup dies
    public float destructionDelay = 0.5f;

    //get animator
    private Animator anim;
    //flag to prevent multiple pickups
    private bool isCollected = false;

    private void Start()
    {
        // Get the Animator from pickup
        anim = GetComponent<Animator>();
    }
    //trigger and no trigger interaction idk why added both
    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcessPickup(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessPickup(collision.gameObject);
    }

    //pickup logic
    private void ProcessPickup(GameObject user)
    {
        if (isCollected) return;

        PlayerHealth healthScript = user.GetComponent<PlayerHealth>();

        if (healthScript != null)
        {
            isCollected = true;
            healthScript.Heal(healAmount);

            StartCoroutine(PlayAnimationAndDestroy());
        }
    }
    //play pickup animation and destroy after delay
    private IEnumerator PlayAnimationAndDestroy()
    {
        if (anim != null)
        {
            anim.SetBool("IsPickuped", true);
        }

        if (GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(destructionDelay);

        Destroy(gameObject);
    }
}