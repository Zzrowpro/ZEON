using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float bulletSpeed = 30f;
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

        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(dmg);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }     
            
        }
    }
}