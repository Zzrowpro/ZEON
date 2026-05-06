using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    public float bulletSpeed;
    public float lifetime;
    public int dmg;

    void Start()
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
            if(obstacle != null)
            {
                obstacle.hp -= dmg;
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }     
            
        }
    }
}
