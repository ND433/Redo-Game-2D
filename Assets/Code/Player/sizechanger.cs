using UnityEngine;

public class PlayerSizeManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public GunIsPistol gunScript; // Directly reference your GunIsPistol script

    [Header("Scales")]
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);
    public Vector3 bigScale = new Vector3(1.7f, 1.7f, 1.7f);

    private void Start()
    {
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        // Automatically find the gun script if not assigned
        if (gunScript == null) gunScript = GetComponentInChildren<GunIsPistol>();
    }

    private void Update()
    {
        if (playerHealth == null) return;

        if (playerHealth.health <= 1f) // TINY STATE
        {
            transform.localScale = normalScale;

            // Disable shooting component
            if (gunScript != null) gunScript.enabled = false;

            // Force ammo to zero
            ForceAmmoToZero();
        }
        else // BIG STATE
        {
            transform.localScale = bigScale;

            // Re-enable shooting
            if (gunScript != null) gunScript.enabled = true;
        }
    }

    void ForceAmmoToZero()
    {
        // Find all guns and set ammoAmount to 0 directly
        GunIsPistol[] guns = GetComponentsInChildren<GunIsPistol>();
        foreach (var gun in guns)
        {
            if (gun.ammoAmount > 0)
            {
                gun.ammoAmount = 0;
                gun.UpdateAmmoDisplay();
            }
        }
    }
}