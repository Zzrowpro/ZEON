using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AggroObstacle : Obstacle
{
    [Header("Aggro Settings")]
    public float aggroSpeed = 7f;

    public Rigidbody2D target;
    private bool inRange;

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            target = player.GetComponentInParent<Rigidbody2D>();
        else
            Debug.LogWarning("AggroObstacle: No GameObject with tag 'Player' found!");
    }

    protected override void HandleMovement()
    {
        if (inRange && target != null)
        {
            Vector2 direction = (target.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * aggroSpeed * Time.fixedDeltaTime);
        }
        else
        {
            base.HandleMovement();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            inRange = false;
    }
}