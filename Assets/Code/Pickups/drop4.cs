using UnityEngine;

public class HeadbuttBlock4 : MonoBehaviour
{
    [Header("Detection")]
    // Reach of the headbutt
    public float hitRange = 1.1f;
    // The tag your blocks must have
    public string targetTag = "Block";

    [Header("Block Visuals")]
    // Trigger name in Animator
    public string animationTrigger = "isHit";
    // The sprite to show once the block is empty
    public Sprite emptyBlockSprite;
    // Sound to play on impact
    public AudioClip hitSound;

    [Header("Drop Settings")]
    // The item to spawn
    public GameObject dropPrefab;
    // Height above the block to spawn it
    public float dropHeight = 1.0f;

    [Header("Editor Visuals")]
    public Color rayColor = Color.yellow;

    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Only trigger when jumping UP
        if (rb.linearVelocity.y > 0)
        {
            // Start ray slightly above player head to avoid hitting self
            Vector2 origin = new Vector2(col.bounds.center.x, col.bounds.max.y + 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, hitRange);

            // If we hit something with the correct Tag
            if (hit.collider != null && hit.collider.CompareTag(targetTag))
            {
                ProcessHit(hit.collider.gameObject, hit.collider);
            }
        }
    }

    void ProcessHit(GameObject blockObj, Collider2D blockCol)
    {
        // 1. Play Sound
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, blockObj.transform.position);
        }

        // 2. Animation
        Animator anim = blockObj.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger(animationTrigger);
        }

        // 3. Swap Sprite to "Empty" version
        SpriteRenderer blockSR = blockObj.GetComponent<SpriteRenderer>();
        if (blockSR != null && emptyBlockSprite != null)
        {
            blockSR.sprite = emptyBlockSprite;
        }

        // 4. Drop Prefab
        if (dropPrefab != null)
        {
            Vector3 spawnPos = new Vector3(blockCol.bounds.center.x, blockCol.bounds.max.y + dropHeight, 0);
            Instantiate(dropPrefab, spawnPos, Quaternion.identity);
        }

        // 5. THE "ONE HIT" FIX
        // By changing the tag to "Untagged", the Raycast will ignore it on the next jump
        blockObj.tag = "Untagged";

        // 6. Bonk Physics
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
    }

    private void OnDrawGizmos()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = rayColor;
            Vector3 start = new Vector3(col.bounds.center.x, col.bounds.max.y + 0.1f, 0);
            Gizmos.DrawLine(start, start + Vector3.up * hitRange);
        }
    }
}