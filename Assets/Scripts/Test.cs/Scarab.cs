using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Scarab : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]private float acceleration = 2f;
    [SerializeField]private float maxSpeed = 5f;
    [SerializeField]private float rotationSpeed = 10f;
    [SerializeField]private float linearDrag = 1f;
    [SerializeField]private float sidewaysDamping = 3f;

    private bool isTargeting = false;
    private bool inRange = false;
    //private bool inRangePlayer = false;
    // Use this when i decide if i want to add player following to the scarab
    [SerializeField]private Transform target;
    //[SerializeField]private Transform playerTransform;
    //Same reason as above. 
    private Rigidbody2D rb; 

    [Header("Shooting")]
    [SerializeField]private GameObject projectilePrefab;
    private float nextFireTime = 0f;
    [SerializeField]private float shootingRate = 0.5f;
    [SerializeField]private Transform firePoint;
    [SerializeField]private Transform firePoint2;

    [Header("WanderingControl")]
    private Transform spawnPosition;
    private float pullStrength = 1f;
    private float wanderStrength = 1f;
    [SerializeField]private float maxWanderDistance = 5f;



    void Awake()
    {
        //[SerializeField]private Transform playerTransform;
        //Same reason as above. 
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform;
    }

    void FixedUpdate()
    {

        if (isTargeting && inRange && target != null)
        {
            HandleThrust();
            HandleRotationTarget();
            KillSidewaysVelocity();
            ClampSpeed();
            HandleShooting();
        }
        else
        {
            HandleThrust();
            KillSidewaysVelocity();
            ClampSpeed();
            HandleRotation();

        }
    }

    

    private void HandleThrust()
    {
        rb.AddForce(transform.up * acceleration);
    }


    private void HandleRotationTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime); 
        rb.MoveRotation(smoothedAngle);
    }

    private void HandleRotation()
    {
        float angle = UnityEngine.Random.insideUnitCircle.normalized.y * Mathf.Rad2Deg - 90f; // Calculate the angle based on a random direction in the unit circle.
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);// Smoothly interpolate the rotation angle towards the target angle using LerpAngle.
        rb.MoveRotation(smoothedAngle);// Apply the smoothed rotation to the Rigidbody2D using MoveRotation.
    }

    private void  KillSidewaysVelocity()
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;

        float forwardSpeed = Vector2.Dot(rb.linearVelocity, forward); // Calculate the forward speed by taking the dot product of the linear velocity and the forward direction.
        float sidewaysSpeed = Vector2.Dot(rb.linearVelocity, right);

        rb.linearVelocity = forward * forwardSpeed + right * sidewaysSpeed * (1f - sidewaysDamping * Time.fixedDeltaTime);
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed) // if the magnitude of the velocity is greater than the max speed, then clamp it to the max speed.  
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed; // Normalize the velocity vector and multiply it by the max speed to clamp it.
        }
    }

    private void HandleShooting()
    {
        if(inRange && isTargeting && nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + shootingRate;
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Instantiate(projectilePrefab, firePoint2.position, firePoint2.rotation);
        }
    }

    private void WanderingControl()
    {
        Vector2 toSpawn = (Vector2)spawnPosition.position - (Vector2)transform.position;
        float distanceFromSpawn = toSpawn.magnitude;
        
        float angle = Mathf.Atan2(toSpawn.y, toSpawn.x) * Mathf.Rad2Deg - 90f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && isTargeting == false)
        {
            isTargeting = true;
            inRange = true;
            target = collision.transform;
        }
    }
}
