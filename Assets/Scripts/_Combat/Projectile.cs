using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed = 30f;
    [SerializeField] private float lifetime = 3;
    [SerializeField] private int dmg = 1;
    [SerializeField] private float knockBack = 15f;
 
    public GameObject explosionEffect;

    protected virtual void  Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Projectile"))
        // {
        //     return;
        // }
        if (collision.isTrigger)
        {
            return;
        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Health obstacle = collision.gameObject.GetComponent<Health>();
            Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            if(obstacle != null)
            {
                obstacle.TakeDamage(dmg, gameObject);
                rigidbody2D.AddForce(transform.up * knockBack, ForceMode2D.Impulse);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }     
            
        }
    }
}
