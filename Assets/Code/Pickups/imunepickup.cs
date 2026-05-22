using UnityEngine;
using System.Collections;

public class PowerUp : PickupBase
{
    [Header("PowerUp Settings")]
    public float duration = 60f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        SetUIText(duration.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            PlayerCollision pc = other.GetComponent<PlayerCollision>();
            if (pc != null)
            {
                StartCoroutine(ActivatePowerUp(pc));
            }
        }
    }

    private IEnumerator ActivatePowerUp(PlayerCollision pc)
    {
        isCollected = true;

        if (col != null) col.enabled = false;

        if (anim != null) anim.SetBool("IsPickuped", true);

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
        }

        pc.isInvincible = true;
        pc.isKillMode = true;

        yield return new WaitForSeconds(destructionDelay);
        if (sr != null) sr.enabled = false;

        float remainingDuration = Mathf.Max(0, duration - destructionDelay);
        yield return new WaitForSeconds(remainingDuration);

        pc.isInvincible = false;
        pc.isKillMode = false;

        Destroy(gameObject);
    }
}