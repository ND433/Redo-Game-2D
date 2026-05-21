using UnityEngine;
using TMPro;

public class GunIsPistol : MonoBehaviour
{
    //gun name
    public string gunName = "Pistol";
    //wat bullet prehab to use
    public GameObject bulletPrefab;
    //what point to shoot from
    public Transform shootPoint;
    //ammo default amt
    public int ammoAmount = 10;
    //max ammo
    public int maxAmmo = 50;
    //bullet goes weeeee or just wee
    public float shootSpeed = 20f;
    //how mush piew piews per shot
    public int bulletsPerShot = 1;
    //how fast peiw pews come out
    public float fireRate = 0.5f;
    //ammo text
    public TextMeshProUGUI ammoText;
    //how fast before pews can shoot again
    private float nextShotTime = 0f;

    void Start()
    {
        //show ammo text
        UpdateAmmoDisplay();
    }

    void Update()
    {
        //if mouse left clikc and have ammo u can do pew pew
        if (Input.GetMouseButton(0) && ammoAmount > 0 && Time.time > nextShotTime)
        {
            //haha u gotta wait for the next shot
            Shoot();
            nextShotTime = Time.time + fireRate;
        }
    }
    //the actual pew pew function
    void Shoot()
    {
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
    //stuff to get more pew pews used for ammopikup
    public void AddAmmo(int amount)
    {
        ammoAmount += amount;
        if (ammoAmount > maxAmmo) ammoAmount = maxAmmo;
        UpdateAmmoDisplay();
    }
    //show how many pew pews u have
    public void UpdateAmmoDisplay()
    {
        if (ammoText != null) ammoText.text = ammoAmount.ToString();
    }
}