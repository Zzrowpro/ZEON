using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Scarab : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float linearDrag = 1f;
    [SerializeField] private float sidewaysDamping = 3f;

    [Header("Player Detection")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float maxDistanceFromPlayer = 40f;
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();

    [Header("Shooting")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootingRate = 0.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private bool canShoot = true;

    [Header("Random Needed Variables")]
    [SerializeField] private float lifetime = 10f;

    private Rigidbody2D rb;
    private Transform target;
    private bool inRange = false;
    private int currentWaypointIndex;
    private float nextFireTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = linearDrag;

        GameObject[] patrolObjects = GameObject.FindGameObjectsWithTag("PatrolPoints");
        foreach (GameObject point in patrolObjects)
        {
            patrolPoints.Add(point.transform);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (target == null) inRange = false;

        HandleThrust();
        KillSidewaysVelocity();
        ClampSpeed();

        bool farFromPlayer = playerTransform != null &&
            Vector2.Distance(playerTransform.position, transform.position) >= maxDistanceFromPlayer;

        canShoot = !farFromPlayer;

        // Only ONE rotation per physics step
        if (farFromPlayer)
        {
            RotateToward(playerTransform.position);
        }
        else if (inRange)
        {
            RotateToward(target.position);
            HandleShooting();
        }
        else
        {
            Patrol();
        }
    }

    private void HandleThrust()
    {
        rb.AddForce(transform.up * acceleration);
    }

    private void RotateToward(Vector2 worldPos)
    {
        Vector2 dir = worldPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        float smoothed = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothed);
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Transform point = patrolPoints[currentWaypointIndex];
        if (Vector2.Distance(transform.position, point.position) < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Count;
        }
        else
        {
            RotateToward(point.position);
        }
    }

    private void KillSidewaysVelocity()
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;

        float forwardSpeed = Vector2.Dot(rb.linearVelocity, forward);
        float sidewaysSpeed = Vector2.Dot(rb.linearVelocity, right);

        float damp = Mathf.Clamp01(1f - sidewaysDamping * Time.fixedDeltaTime);
        rb.linearVelocity = forward * forwardSpeed + right * sidewaysSpeed * damp;
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void HandleShooting()
    {
        if (nextFireTime <= Time.time && canShoot && ObstacleAhead())
        {
            nextFireTime = Time.time + shootingRate;
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Instantiate(projectilePrefab, firePoint2.position, firePoint2.rotation);
        }
    }

    private bool ObstacleAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, detectionRadius * 2f, obstacleMask);
        return hit.collider != null;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (target == null && collision.CompareTag("Obstacle"))
        {
            target = collision.transform;
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == target)
        {
            target = null;
            inRange = false;
        }
    }
}