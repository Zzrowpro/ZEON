using System.Collections;
using UnityEngine;
public class Bomb : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float time = 2f;
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;

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
        CheckSurroundings();
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
            if(effect)Instantiate(effect, transform.position, transform.rotation);
            Destroy(gameObject, 0.1f);
        }
}