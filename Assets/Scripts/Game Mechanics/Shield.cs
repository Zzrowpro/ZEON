using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Health))]

public class Shield : MonoBehaviour
{
    [SerializeField]private Transform player;
    //public GameObject projectilePrefab;
    [SerializeField]private float knockBack = 1f;
    [SerializeField]private int damageGiven = 2;


    private Health shieldHealth;
    private SpriteRenderer sr;

    public float speed = 0.2f;
    private Color c;
    

    void Awake()
    {
        shieldHealth = GetComponent<Health>();
        sr = GetComponent<SpriteRenderer>();
        c = GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        
        transform.position = new Vector2(player.position.x, player.position.y);
        transform.rotation = Quaternion.Euler(0f, 0f, player.eulerAngles.z);

        /*
        float hue = Mathf.Repeat(Time.time * speed, 1f);
        float alpha = Mathf.PingPong(Time.time * speed, 1f);
        Color c = Color.HSVToRGB(hue, 1f, 1f);
        c.a = alpha;
        sr.color = c;
        */

        //Sheild is gonna be a sprite instead of looking like a power up.

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
            health.TakeDamage(damageGiven);
            shieldHealth.TakeDamage(1);

            rb.AddForce(transform.up * knockBack, ForceMode2D.Impulse);
        }
    }
}
