using UnityEngine;

public class HealingPickup : PickupBase
{
    [Header("Healing Settings")]
    public float healAmount = 10f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        SetUIText(healAmount.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcessPickup(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessPickup(collision.gameObject);
    }

    private void ProcessPickup(GameObject user)
    {
        if (isCollected) return;

        if (user.CompareTag("Player"))
        {
            PlayerHealth healthScript = user.GetComponent<PlayerHealth>();
            if (healthScript != null)
            {
                healthScript.Heal(healAmount);
                StartCoroutine(PlayEffectsAndDestroy());
            }
        }
    }
}