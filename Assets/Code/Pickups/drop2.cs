using UnityEngine;

public class HeadbuttBlock2 : MonoBehaviour
{
    [Header("Detection")]
    public float hitRange = 1.1f;
    public string targetTag = "Block";

    [Header("Block Visuals")]
    public string animationTrigger = "isHit";
    public Sprite emptyBlockSprite;
    public AudioClip hitSound;

    [Header("Drop Settings")]
    public GameObject dropPrefab;
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
        if (rb.linearVelocity.y > 0)
        {
            Vector2 origin = new Vector2(col.bounds.center.x, col.bounds.max.y + 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, hitRange);

            if (hit.collider != null && hit.collider.CompareTag(targetTag))
            {
                ProcessHit(hit.collider.gameObject, hit.collider);
            }
        }
    }

    void ProcessHit(GameObject blockObj, Collider2D blockCol)
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, blockObj.transform.position);
        }

        Animator anim = blockObj.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger(animationTrigger);
        }

        SpriteRenderer blockSR = blockObj.GetComponent<SpriteRenderer>();
        if (blockSR != null && emptyBlockSprite != null)
        {
            blockSR.sprite = emptyBlockSprite;
        }

        if (dropPrefab != null)
        {
            Vector3 spawnPos = new Vector3(blockCol.bounds.center.x, blockCol.bounds.max.y + dropHeight, 0);
            Instantiate(dropPrefab, spawnPos, Quaternion.identity);
        }

        blockObj.tag = "Untagged";

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