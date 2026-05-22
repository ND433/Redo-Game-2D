using UnityEngine;
using System.Collections;
using TMPro;

public abstract class PickupBase : MonoBehaviour
{
    [Header("Base Settings")]
    public float destructionDelay = 0.5f;

    [Header("Audio Settings")]
    public AudioClip pickupSound;
    [Range(0, 1)] public float volume = 1f;

    [Header("Movement / Drag Settings")]
    public bool isDragging = false;
    public float moveSpeed = 5f;

    [Header("UI Auto-Finder Setup")]
    [Tooltip("Type the exact name of the TextMeshPro UI Game Object in your scene that this pickup should update.")]
    public string targetSceneTextName = "GemScoreText";

    protected TMP_Text textComponent;

    protected Animator anim;
    protected Collider2D col;
    protected SpriteRenderer sr;
    protected Transform playerTransform;
    protected bool isCollected = false;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (!string.IsNullOrEmpty(targetSceneTextName))
        {
            GameObject foundTextObj = GameObject.Find(targetSceneTextName);
            if (foundTextObj != null)
            {
                textComponent = foundTextObj.GetComponent<TMP_Text>();
            }
        }

        UpdateUI();
    }

    protected virtual void Update()
    {
        if (isDragging && playerTransform != null && !isCollected)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    protected virtual void UpdateUI()
    {
    }

    protected void SetUIText(string textValue)
    {
        if (textComponent != null)
        {
            textComponent.text = textValue;
        }
    }

    protected virtual IEnumerator PlayEffectsAndDestroy()
    {
        isCollected = true;

        if (col != null) col.enabled = false;

        if (anim != null)
        {
            anim.SetBool("IsPickuped", true);
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
        }

        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }
}