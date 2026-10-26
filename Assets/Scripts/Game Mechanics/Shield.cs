using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Health))]

public class Shield : MonoBehaviour
{
    private Transform player;
    //public GameObject projectilePrefab;
    [SerializeField]private float knockBack = 1f;
    [SerializeField]private int damage = 2;


    private Health shieldHealth;

    public float speed = 0.2f;
    

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        shieldHealth = GetComponent<Health>();
    }

    void Update()
    {     
        transform.position = new Vector2(player.position.x, player.position.y);
        transform.rotation = Quaternion.Euler(0f, 0f, player.eulerAngles.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Projectile"))
        {
            return;
        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Health health = collision.gameObject.GetComponent<Health>();
            health.TakeDamage(damage, gameObject);
            shieldHealth.TakeDamage(1, collision.gameObject);
            rb.AddForce(transform.up * knockBack, ForceMode2D.Impulse);
        }
    }
}
