using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AggroObstacle : Obstacle
{
    [Header("Aggro Settings")]
    public float aggroSpeed = 7f;

    public Transform target;
    public bool inRange;

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            target = player.GetComponentInParent<Transform>();
        else
            Debug.LogWarning("AggroObstacle: No GameObject with tag 'Player' found!");
    }

    protected override void HandleMovement()
    {
        if (inRange && target != null)
        {
            Debug.Log("AHHHH");
            Vector2 targetPosition = target.position;
            Vector2 currentPosition = transform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            rb.MovePosition(currentPosition + (direction * aggroSpeed * Time.deltaTime));
        }
        else
        {
            base.HandleMovement();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Halt");
        if (collision.gameObject.CompareTag("Player"))
            inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            inRange = false;
    }
}