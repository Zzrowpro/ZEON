using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minimum = 0.5f;
    public float maximum = 2.0f;

    public float baseSpeed = 5f; 
    public float aimlessStrength = 2f;   // HOW chaotic they move
    public float directionChangeSpeed = 1f; // HOW often they change direction

    public float maxSpinSpeed = 10f;

    private Rigidbody2D rb;
    private Vector2 currentDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Random Scale
        float ranScale = Random.Range(minimum, maximum);
        transform.localScale = new Vector3(ranScale, ranScale, 1);

        // Initial direction
        currentDirection = Random.insideUnitCircle.normalized;

        // Random Spin
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    void FixedUpdate()
    {
        // Smoothly change direction over time
        Vector2 randomOffset = Random.insideUnitCircle * aimlessStrength;
        currentDirection = Vector2.Lerp(currentDirection, randomOffset, directionChangeSpeed * Time.fixedDeltaTime);

        rb.AddForce(currentDirection.normalized * baseSpeed);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponentInParent<Health>();
        if (collision.gameObject.CompareTag("Player"))
        {
            health.hp--;
            gameObject.GetComponent<Health>().hp--;
        }
    }
}