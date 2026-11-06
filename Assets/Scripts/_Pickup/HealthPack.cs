using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class HealthPack : MonoBehaviour
{
    [SerializeField]private int hpInc;
    [SerializeField]private float acceleration = 2f;
    [SerializeField]private float maxSpeed = 5f;

    private Rigidbody2D rb;

    private float lifespan = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 direction = BezierUtility.BezierPoint(rb.position, rb.position, Random.insideUnitCircle * 10, rb.position,1f);
        rb.MovePosition(direction);
    } 


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Health player = collision.gameObject.GetComponent<Health>();
            player.HpIncrease(hpInc);
        }
    }
}
