using UnityEngine;

public class PlayerSizeManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public GunIsPistol gunScript;

    [Header("Scales")]
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);
    public Vector3 bigScale = new Vector3(1.7f, 1.7f, 1.7f);

    private void Start()
    {
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (gunScript == null) gunScript = GetComponentInChildren<GunIsPistol>();
    }

    private void Update()
    {
        if (playerHealth == null) return;

        if (playerHealth.health <= 1f) 
        {
            transform.localScale = normalScale;

            if (gunScript != null) gunScript.enabled = false;

            ForceAmmoToZero();
        }
        else 
        {
            transform.localScale = bigScale;

            if (gunScript != null) gunScript.enabled = true;
        }
    }

    void ForceAmmoToZero()
    {
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