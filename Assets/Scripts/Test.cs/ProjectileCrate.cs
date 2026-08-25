using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileCrate : MonoBehaviour, IMimicable
{
    public float bulletSpeed;
    public float lifetime;
    public int dmg;

    private Transform target;
    private Rigidbody2D rb; 

    public void CopyStateFrom(IMimicable other)
    {
        if (other is Shooterobstacles a)
        {
            transform.localScale = a.transform.localScale; // mimics size too
            target = a.target;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 direction = (Vector2)target.position -(Vector2) transform.position;
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponentInParent<Health>();
            health.TakeDamage(dmg);
            Destroy(gameObject);
        }
    }

}
