using UnityEngine;

public class GemPickup : PickupBase
{
    [Header("Gem Settings")]
    public int scoreValue = 1;

    private static int totalScore = 0;

    public static void ResetScore()
    {
        totalScore = 0;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        SetUIText(totalScore.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            totalScore += scoreValue;
            UpdateUI();
            StartCoroutine(PlayEffectsAndDestroy());
        }
    }
}