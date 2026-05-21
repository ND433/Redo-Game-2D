using UnityEngine;
using System.Collections;

public class PistolAmmoPickup : MonoBehaviour
{
    //gunname
    public string targetGunName = "Pistol";
    //start ammo
    public int ammoToGive = 15;
    //bullet shoot speed
    public float moveSpeed = 5f;
    //if ammo gets drag t player default false 
    public bool isDragging = false;
    //delay before destroy after pickup
    public float destructionDelay = 0.5f; 

    //get player position
    private Transform playerTransform;
    //get animator
    private Animator anim;
    //flag to prevent multiple pickups
    private bool isCollected = false;

    void Start()
    {
        // Get the Animator of player
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Move towards player if dragging
    void Update()
    {
        if (isDragging && playerTransform != null && !isCollected)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    // Detect collision with player
    void OnTriggerEnter2D(Collider2D other)
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
                    StartCoroutine(PlayAnimationAndDestroy());
                    break;
                }
            }
        }
    }
    // Play pickup animation and destroy the pickup after a delay
    private IEnumerator PlayAnimationAndDestroy()
    {
        isCollected = true;

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