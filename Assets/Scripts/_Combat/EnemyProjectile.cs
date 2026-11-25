using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    public float bulletSpeed = 30f;
    private bool hit = false;
    [SerializeField] protected float lifetime = 3;
    [SerializeField] protected GameObject hitVFX;
    [SerializeField] private int dmg = 1;

    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(Delete());
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
                hit = true;

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

    protected virtual IEnumerator Delete()
    {
        yield return new WaitForSeconds(lifetime);

        if (!hit && hitVFX != null)
        {
            GameObject vfx = Instantiate(hitVFX,transform.position,Quaternion.identity);
            Destroy(vfx,3);
        }

        Destroy(gameObject);
    }
}