using UnityEngine;

public class PlayerSizeManager : MonoBehaviour
{
    // This creates a slot in the inspector to safely link your health script
    [Header("References")]
    public PlayerHealth playerHealth;

    [Header("Scales")]
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);
    public Vector3 bigScale = new Vector3(1.7f, 1.7f, 1.7f);

    private void Start()
    {
        // Automatically look for the script on the same GameObject if you forgot to drag it
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        // Safety check: if the health script isn't found yet, do nothing to prevent errors
        if (playerHealth == null) return;

        // Halfway point calculation (50 if maxHealth is 100)
        float threshold = playerHealth.maxHealth / 2f;

        // Check health value and swap scales cleanly
        if (playerHealth.health <= threshold)
        {
            // HP State 1: Tiny
            transform.localScale = normalScale;
        }
        else
        {
            // HP State 2: Big
            transform.localScale = bigScale;
        }
    }
}