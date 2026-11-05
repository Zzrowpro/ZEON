using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovement : MonoBehaviour
{
    public float minimum = 0.5f;
    public float maximum = 2.0f;

    public float baseSpeed = 5f;
    public float aimlessStrength = 2f;
    public float directionChangeSpeed = 1f;
    public float maxSpinSpeed = 10f;
    public float roamRadius = 3f;

    private Vector2 spawnPosition;
    private Vector2 currentDirection;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        currentDirection = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }


    protected void HandleMovement()
    {
        Vector2 toSpawn = spawnPosition - (Vector2)transform.position;
        float distanceFromSpawn = toSpawn.magnitude;

        Vector2 randomOffset = Random.insideUnitCircle * aimlessStrength;

        float pullStrength = distanceFromSpawn / roamRadius;
        Vector2 targetDirection = Vector2.Lerp(randomOffset, toSpawn.normalized, pullStrength);

        currentDirection = Vector2.Lerp(currentDirection, targetDirection, directionChangeSpeed * Time.fixedDeltaTime); //smoothing out the pull in target direction

        rb.AddForce(currentDirection.normalized * baseSpeed);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, baseSpeed);
    }
}