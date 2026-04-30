using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minimum = 0.5f;
    public float maximum = 2.0f;

    public float baseSpeed = 5f;
    public float aimlessStrength = 2f;
    public float directionChangeSpeed = 1f;
    public float maxSpinSpeed = 10f;
    public float roamRadius = 3f;

    protected Rigidbody2D rb;
    protected Vector2 currentDirection;
    private Vector2 spawnPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float ranScale = Random.Range(minimum, maximum);
        transform.localScale = new Vector3(ranScale, ranScale, 1);

        spawnPosition = transform.position;
        currentDirection = Random.insideUnitCircle.normalized;

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);

        OnStart();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    protected virtual void OnStart()
    {
        
    }

    protected virtual void HandleMovement()
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponentInParent<Health>();
            if (playerHealth != null)
                playerHealth.hp--;

            Health selfHealth = GetComponent<Health>();
            if (selfHealth != null)
                selfHealth.hp--;
        }
    }
}