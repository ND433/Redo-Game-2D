using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class colliderloadscenecoords : MonoBehaviour
{
    [Header("Transition Settings")]
    public float transitionDelay = 5.0f; 
    public string animationTriggerName = "Teleport";

    [Header("Audio Settings")]
    public AudioClip transitionSound;
    [Range(0f, 1f)] public float volume = 1f;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(SequenceTeleport(collision.gameObject));
        }
    }

    private IEnumerator SequenceTeleport(GameObject player)
    {
        isTransitioning = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; 
        }

        MonoBehaviour movementScript = player.GetComponent("MovementForPlatformer") as MonoBehaviour;
        if (movementScript != null) movementScript.enabled = false;

        if (transitionSound != null)
        {
            AudioSource.PlayClipAtPoint(transitionSound, player.transform.position, volume);
        }

        Animator anim = player.GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            anim.Rebind();
            anim.SetTrigger(animationTriggerName);
        }

        yield return new WaitForSeconds(transitionDelay);

        player.transform.position = new Vector3(34.6029f, -33.7788f, 0f);

        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        if (movementScript != null) movementScript.enabled = true;

        isTransitioning = false;
    }
}