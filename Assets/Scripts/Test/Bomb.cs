using System.Collections;
using UnityEngine;
public class Bomb : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float time = 2f;
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;
    [SerializeField] private float effectLifetime = 2f;
    [SerializeField] private bool targetPlayer = false;
 
    [Header("Range")]
    [SerializeField] private int subdivisions = 10;
    [SerializeField] private float radius = 5f;
    [SerializeField] private LineRenderer lineRenderer;

    private readonly Collider2D[] hitColliders = new Collider2D[32];

    void Start()
    {
        StartCoroutine(WaitForExplosionCoroutine());
    }

    void Update()
    {
        float angleStep = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;
        for(int i  = 0; i < subdivisions; i++)
        {
            float xPosition = radius * Mathf.Cos(angleStep * i);
            float yPosition = radius * Mathf.Sin(angleStep * i);

            Vector3 pointInCircle = new Vector3(xPosition, yPosition);
            lineRenderer.SetPosition(i, pointInCircle);
        }

    }

    private IEnumerator WaitForExplosionCoroutine()
    {
        yield return new WaitForSeconds(time);
        if(targetPlayer == false)CheckSurroundings();
        else CheckForPlayer();
    }

    void CheckSurroundings()
    {
        int numColliders = Physics2D.OverlapCircle(transform.position, radius, ContactFilter2D.noFilter, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }
        if (effect)
        {
            GameObject fx = Instantiate(effect, transform.position, transform.rotation);
            Destroy(fx, effectLifetime);
        }
        Destroy(gameObject, effectLifetime);
    }

    void CheckForPlayer()
    {
        int numColliders = Physics2D.OverlapCircle(transform.position, radius, ContactFilter2D.noFilter, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag("Player"))
            {
                hitColliders[i].TryGetComponent(out PlayerHealth playerHealth);
                playerHealth.TakeDamage(damage);
                break;
            }
        }
        GameObject fx = Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}