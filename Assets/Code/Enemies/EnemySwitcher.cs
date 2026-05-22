using UnityEngine;

public class EnemySwitcher : MonoBehaviour
{
    [Header("Movement & Waypoints")]
    public GameObject pathObject;
    public float patrolSpeed = 3f;
    public float breakTime = 1.0f;

    [Header("AI & Chase Logic")]
    public bool canChase = true;
    public float returnToPathDelay = 1.5f;
    
    private Transform player;
    private EnemyChase chaseScript;
    private EnemyWaypoints pathScript;

    private int waypointIndex = 0;
    private float breakTimer;
    private float chaseCooldownTimer;
    private bool wasChasing;

    void Start()
    {
        chaseScript = GetComponent<EnemyChase>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (pathObject != null)
        {
            pathScript = pathObject.GetComponent<EnemyWaypoints>();
        }
    }

    void Update()
    {
        bool isChasingNow = false;

        if (canChase && chaseScript != null && player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= chaseScript.detectionRange)
            {
                isChasingNow = true;
                wasChasing = true;
                chaseCooldownTimer = returnToPathDelay;

                chaseScript.Move(player.position, chaseScript.speed);
            }
        }

        if (!isChasingNow)
        {
            if (wasChasing)
            {
                chaseCooldownTimer -= Time.deltaTime;
                if (chaseCooldownTimer <= 0) wasChasing = false;
                return;
            }

            MoveOnPath();
        }
    }
    void MoveOnPath()
    {
        if (pathScript == null) return;
        Transform[] nodes = pathScript.GetWaypoints();
        if (nodes.Length == 0) return;

        if (breakTimer > 0)
        {
            breakTimer -= Time.deltaTime;
            return;
        }

        Transform target = nodes[waypointIndex];

        if (chaseScript != null)
        {
            chaseScript.Move(target.position, patrolSpeed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % nodes.Length;
            breakTimer = breakTime;
        }
    }
}