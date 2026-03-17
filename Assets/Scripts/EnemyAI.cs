// Centrale AI controller voor de enemy. Beheert state transitions en detectie van de speler.

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // State system
    private IEnemyState currentState;
    private Vector3 currentVelocity = Vector3.zero;
    
    // References
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    
    // Waypoints voor patrouille
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    
    // Detection parameters
    [SerializeField] private float sightRange = 15f;
    [SerializeField] private float sightAngle = 90f; // FOV in graden
    [SerializeField] private float hearingRange = 10f;
    [SerializeField] private float captureRange = 1f; // Afstand om speler te pakken
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    
    // Audio parameters voor hearing
    private float noiseLevel = 0f;
    [SerializeField] private float noiseDecayRate = 2f; // Hoe snel geluid verdwijnt
    
    // Game Over handling
    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
        
        // Find player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        
        // Initialiseer in IdleState
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        // Update geluidsniveau (verval in de tijd)
        if (noiseLevel > 0)
        {
            noiseLevel -= noiseDecayRate * Time.deltaTime;
            noiseLevel = Mathf.Max(0, noiseLevel);
        }
        
        // Voer huidige state uit
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    /// <summary>
    /// Verandert de huidige state en voert Enter/Exit uit
    /// </summary>
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>
    /// Controleert of de enemy de speler kan zien
    /// </summary>
    public bool CanSeePlayer()
    {
        if (playerTransform == null)
            return false;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check distance
        if (distanceToPlayer > sightRange)
            return false;

        // Check FOV (field of view)
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > sightAngle / 2f)
            return false;

        // Check line of sight (raycast)
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer, out hit, sightRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Controleert of de enemy geluid van speler kan horen
    /// </summary>
    public bool CanHearPlayer()
    {
        return noiseLevel > 0;
    }

    /// <summary>
    /// Voegt geluidsniveau toe (bv. wanneer speler loopt)
    /// </summary>
    public void DetectNoise(float amount, Vector3 noisePosition)
    {
        float distance = Vector3.Distance(transform.position, noisePosition);
        
        if (distance <= hearingRange)
        {
            noiseLevel += amount;
            noiseLevel = Mathf.Min(100f, noiseLevel); // Cap op 100
        }
    }

    /// <summary>
    /// Controleert of speler dicht genoeg is om gepakt te worden
    /// </summary>
    public bool IsPlayerInRange()
    {
        if (playerTransform == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= captureRange;
    }

    /// <summary>
    /// Voert patrouille uit tussen waypoints
    /// </summary>
    public void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            navMeshAgent.velocity = Vector3.zero;
            return;
        }

        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        // Check of waypoint bereikt is
        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= stoppingDistance)
        {
            if (!navMeshAgent.hasPath || !navMeshAgent.pathPending)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    /// <summary>
    /// Voert aanval uit op speler
    /// </summary>
    public void AttackPlayer()
    {
        if (playerTransform == null)
            return;

        navMeshAgent.speed = attackSpeed;
        navMeshAgent.SetDestination(playerTransform.position);
    }

    /// <summary>
    /// Handlert game over wanneer speler wordt gepakt
    /// </summary>
    public void CapturePlayer()
    {
        Debug.Log("Player captured! Game Over!");
        OnGameOver?.Invoke();
        Time.timeScale = 0; // Pauzeer het spel
    }

    /// <summary>
    /// Krijgt de huidige speler transform
    /// </summary>
    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    /// <summary>
    /// Krijgt de sight range (voor debugging/gizmos)
    /// </summary>
    public float GetSightRange()
    {
        return sightRange;
    }

    /// <summary>
    /// Tekent debug gegevens in de Scene view
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetSightRange());

        // Hearing range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        // Capture range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, captureRange);

        // Waypoints
        if (waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.3f);
                
                // Lijn naar volgende waypoint
                Gizmos.DrawLine(waypoints[i].position, 
                    waypoints[(i + 1) % waypoints.Length].position);
            }
        }
    }
}
