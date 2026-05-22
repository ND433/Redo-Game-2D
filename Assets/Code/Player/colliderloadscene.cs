using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class colliderloadscene : MonoBehaviour
{
    [Header("Transition Settings")]
    public float transitionDelay = 5.0f; 
    public string animationTriggerName = "EnterDoor";

    [Header("Audio Settings")]
    public AudioClip transitionSound;
    [Range(0f, 1f)] public float volume = 1f;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(SequenceSceneLoad(collision.gameObject));
        }
    }

    private IEnumerator SequenceSceneLoad(GameObject player)
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

        SceneManager.LoadScene("1-2");
    }
}