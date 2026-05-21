using UnityEngine;
using System.Collections;
using TMPro;

public class GemPickup : MonoBehaviour
{
    //gem score u get per 1 gem
    public int scoreValue = 1;
    //time before gem dies after being picked up
    public float destructionDelay = 0.5f;

    [Header("Audio Settings")]
    // Line 1: Slot for the pickup sound
    public AudioClip pickupSound;
    // Line 2: Control the volume
    [Range(0, 1)] public float volume = 1f;

    [Header("Drag any TMP Text object here")]
    //what text to show score on
    public GameObject scoreTextObject;

    //get animator
    private Animator anim;
    //to prevent multiple pickups of the same gem
    private bool isCollected = false;

    //makes it that default score is 0
    private static int totalScore = 0;

    private void Awake()
    {
    }
    //makes reset on scene reload
    public static void ResetScore()
    {
        totalScore = 0;
    }

    private void Start()
    {
        //get animator of gem
        anim = GetComponent<Animator>();

        UpdateUI();
    }

    //on trigger and non pickup
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            totalScore += scoreValue;

            UpdateUI();

            // Line 3: Play the sound at the gem's position
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
            }

            if (anim != null)
            {
                anim.SetBool("IsPickuped", true);
            }

            if (GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().enabled = false;

            StartCoroutine(Cleanup());
        }
    }

    //update gem score text
    private void UpdateUI()
    {
        if (scoreTextObject != null)
        {
            TMP_Text textComponent = scoreTextObject.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = totalScore.ToString();
            }
        }
    }
    //wait before dying to make animations and stuff work
    private IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }
}