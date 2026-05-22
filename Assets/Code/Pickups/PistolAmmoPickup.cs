using UnityEngine;

public class PistolAmmoPickup : PickupBase
{
    [Header("Ammo Settings")]
    public string targetGunName = "Pistol";
    public int ammoToGive = 15;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        SetUIText(ammoToGive.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            GunIsPistol[] allGuns = other.GetComponentsInChildren<GunIsPistol>();
            foreach (GunIsPistol gun in allGuns)
            {
                if (gun.gunName == targetGunName)
                {
                    gun.AddAmmo(ammoToGive);
                    StartCoroutine(PlayEffectsAndDestroy());
                    break;
                }
            }
        }
    }
}