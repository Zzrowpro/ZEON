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
    private float timeLeft = 0f;
    public Color onColor;
    public Color offColor;
    

    void Awake()
    {
        shieldHealth = gameObject.GetComponent<Health>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = new Vector2(player.position.x, player.position.y);
        transform.rotation = Quaternion.Euler(0f, 0f, player.eulerAngles.z); 


        
        Color c = Color.Lerp(onColor, sr.color, timeLeft);
              sr.color = new Color(c.r, c.g, c.b, 3);
              Debug.Log($"{c.r}, {c.g}, {c.b}");

    }

    public void OCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            health.hp -= 1;

            
            shieldHealth.hp -= 1;

            rb.AddForce(transform.up * kB, ForceMode2D.Impulse);
        }
    }
}
