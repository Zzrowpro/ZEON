using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Pyro : MonoBehaviour
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
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawnPosition = transform.position;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        animator.SetFloat("xMove",currentDirection.x);
        animator.SetFloat("yMove",currentDirection.y);
    }

    private void HandleMovement()
    {
        animator.SetBool("isMoving", true);
        Vector2 toSpawn = spawnPosition - (Vector2)transform.position;//Calculating the distance the gameobject is from its spawn position
        float distanceFromSpawn = toSpawn.magnitude; //Marking the distance we are from the gameobjects spawn position

        Vector2 randomOffset = Random.insideUnitCircle * aimlessStrength;
        float pullStrength = distanceFromSpawn / roamRadius; // Dividing the 2 gives us a ratio : 0 close to spawn and 1 close to the border of the roam radius
        Vector2 targetDirection = Vector2.Lerp(randomOffset, toSpawn.normalized,pullStrength ); //Makes the enemy wander freely near its spawn position but slowly pulls back as it reaches the border.

        currentDirection = Vector2.Lerp(currentDirection, targetDirection, directionChangeSpeed * Time.deltaTime); // Smooths out the directional change of target direction

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, baseSpeed);
        rb.AddForce(currentDirection.normalized * baseSpeed);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, baseSpeed);
    }

    //private IEnumerator HandleMovementNew()
    //{
        
    //}

}
