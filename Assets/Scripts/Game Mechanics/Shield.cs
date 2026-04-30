using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Health))]

public class Shield : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public float kB = 1f;
    Health shieldHealth;
    private SpriteRenderer sr;
    public float speed = 0.2f;
    private Color c;
    

    void Awake()
    {
        shieldHealth = gameObject.GetComponent<Health>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        c = gameObject.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        
        transform.position = new Vector2(player.position.x, player.position.y);
        transform.rotation = Quaternion.Euler(0f, 0f, player.eulerAngles.z);
        float hue = Mathf.Repeat(Time.time * speed, 1f);
        float alpha = Mathf.PingPong(Time.time * speed, 1f);
        Color c = Color.HSVToRGB(hue, 1f, 1f);
        c.a = alpha;
        sr.color = c;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Destroy(collision.gameObject);
            shieldHealth.hp -= 1;

            rb.AddForce(transform.up * kB, ForceMode2D.Impulse);
        }
    }
}
