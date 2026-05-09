using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Fuel : MonoBehaviour
{

    [Header("Pathing")]
    public float baseSpeed = 5f;
    public float aimlessStrength = 2f;
    public float directionChangeSpeed = 1f;
    public float maxSpinSpeed = 10f;
    public float roamRadius = 3f;

    [Header("Attributes")]
    public float fuelAmount;

    protected Vector2 currentDirection;
    protected Rigidbody2D rb; 
    private Vector2 spawnPosition;

    
    
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        spawnPosition = transform.position;
        currentDirection = Random.insideUnitCircle.normalized;

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 toSpawn = spawnPosition - (Vector2)transform.position;
        float distanceFromSpawn = toSpawn.magnitude;

        Vector2 randomOffset = Random.insideUnitCircle * aimlessStrength;

        float pullStrength = distanceFromSpawn / roamRadius;
        Vector2 targetDirection = Vector2.Lerp(randomOffset, toSpawn.normalized, pullStrength);

        currentDirection = Vector2.Lerp(currentDirection, targetDirection, directionChangeSpeed * Time.fixedDeltaTime);

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, baseSpeed);
        rb.AddForce(currentDirection.normalized * baseSpeed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           PlayerController pr = collision.GetComponent<PlayerController>();
           pr.fuel += fuelAmount;
           Destroy(gameObject);
        }
    } 

}