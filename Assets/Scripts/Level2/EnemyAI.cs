
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform[] patrolPoints;
    public Animator animator;

    [Header("Patrulla")]
    public float patrolSpeed = 2f;
    public float waitTime = 2f;

    [Header("Persecución")]
    public float chaseSpeed = 4.5f;
    public float detectionRange = 8f;
    public float loseRange = 12f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private float waitTimer = 0f;
    private float chaseTimer = 0f;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;

        if (player == null)
        {
            GameObject obj =
                GameObject.FindGameObjectWithTag("Player");

            if (obj != null)
                player = obj.transform;
        }

        GoToNextPoint();
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (!chasing && distance <= detectionRange)
        {
            chasing = true;
            agent.speed = chaseSpeed;
            agent.stoppingDistance = 1.2f;
            chaseTimer = 0.2f;
        }

        if (chasing)
        {
            if (distance >= loseRange)
            {
                chasing = false;
                agent.speed = patrolSpeed;
                agent.stoppingDistance = 0.3f;
                waitTimer = 0f;
                GoToNextPoint();
            }
            else
            {
                chaseTimer += Time.deltaTime;

                if (chaseTimer >= 0.2f)
                {
                    agent.SetDestination(player.position);
                    chaseTimer = 0f;
                }
            }
        }
        else
        {
            Patrol();
        }

        UpdateAnimation();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <=
            agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                currentPoint =
                    (currentPoint + 1) % patrolPoints.Length;

                GoToNextPoint();
                waitTimer = 0f;
            }
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0 ||
            !agent.isOnNavMesh)
            return;

        Transform point = patrolPoints[currentPoint];

        if (point != null)
            agent.SetDestination(point.position);
    }

    void UpdateAnimation()
    {
        if (animator == null)
            return;

        float speed = agent.velocity.magnitude;

        float value = 0f;

        if (speed > 0.1f)
            value = chasing ? 1f : 0.5f;

        animator.SetFloat(
            "Speed",
            value,
            0.1f,
            Time.deltaTime
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            loseRange
        );
    }
}
