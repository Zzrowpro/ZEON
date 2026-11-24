using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    public float bulletSpeed = 30f;
    [SerializeField] protected float lifetime = 3;
    [SerializeField] protected GameObject hitVFX;
    [SerializeField] private int dmg = 1;
 
    protected virtual void  Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            return;
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out PlayerHealth playerHealth);
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dmg,gameObject);

                if (hitVFX != null)
                {
                    GameObject vfx = Instantiate(hitVFX,transform.position,Quaternion.identity);
                    Destroy(vfx,3);
                }

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }     
            
        }
    }
}