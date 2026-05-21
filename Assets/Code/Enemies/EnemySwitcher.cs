using UnityEngine;

public class EnemySwitcher : MonoBehaviour
{
    [Header("Movement & Waypoints")]
    //what waypoint to follow
    public GameObject pathObject;
    //thing at waypoint speeeeed
    public float patrolSpeed = 3f;
    //u gotta wait before u go to next
    public float breakTime = 1.0f;

    [Header("AI & Chase Logic")]
    //if thing switch between chasing and patrolling
    public bool canChase = true;
    //if out of range return to path time
    public float returnToPathDelay = 1.5f;
    
    //stuff
    private Transform player;
    private EnemyChase chaseScript;
    private EnemyWaypoints pathScript;

    //how much waypoints
    private int waypointIndex = 0;
    private float breakTimer;
    private float chaseCooldownTimer;
    private bool wasChasing;

    void Start()
    {
        //get the chase script and player position
        chaseScript = GetComponent<EnemyChase>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (pathObject != null)
        {
            pathScript = pathObject.GetComponent<EnemyWaypoints>();
        }
    }

    void Update()
    {
        //if in range chase if not follow path if was on chase after time back to path
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
    //move to waypoints and wait at them before go to next
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