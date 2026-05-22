using UnityEngine;
using TMPro;

public class GunIsPistol : MonoBehaviour
{
    public string gunName = "Pistol";
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public int ammoAmount = 10;
    public int maxAmmo = 50;
    public float shootSpeed = 20f;
    public int bulletsPerShot = 1;
    public float fireRate = 0.5f;

    [Header("Audio Settings")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 1f;

    public TextMeshProUGUI ammoText;
    private float nextShotTime = 0f;

    void Start()
    {
        UpdateAmmoDisplay();
    }

    void Update()
    {
        //if (Input.GetMouseButton(0) && ammoAmount > 0 && Time.time > nextShotTime)
        if (Input.GetKey(KeyCode.S) && ammoAmount > 0 && Time.time > nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + fireRate;
        }
    }
    void Shoot()
    {
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position, shootVolume);
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

            float facingDirection = Mathf.Sign(transform.root.localScale.x);
            Vector2 bulletDirection = new Vector2(facingDirection, 0);

            rb.linearVelocity = bulletDirection * shootSpeed;

            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            Destroy(newBullet, 2.0f);
            ammoAmount--;
        }
        UpdateAmmoDisplay();
    }
    public void AddAmmo(int amount)
    {
        ammoAmount += amount;
        if (ammoAmount > maxAmmo) ammoAmount = maxAmmo;
        UpdateAmmoDisplay();
    }
    public void UpdateAmmoDisplay()
    {
        if (ammoText != null) ammoText.text = ammoAmount.ToString();
    }
}