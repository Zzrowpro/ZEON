using System.Collections;
using UnityEngine;
public class Bomb : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float duration = 2f;
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;
    [SerializeField] private float effectLifetime = 2f;
    [SerializeField] private bool targetPlayer = false;
 
    [Header("Range")]
    [SerializeField] private int subdivisions = 10;
    [SerializeField] private float radius = 5f;
    [SerializeField] private LineRenderer lineRenderer;

    private static readonly int FlashAmountID = Shader.PropertyToID("_FlashAmount");
    private readonly Collider2D[] hitColliders = new Collider2D[32];
    private Material material;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    void Start()
    {
        lineRenderer.loop = true;
        lineRenderer.positionCount = subdivisions;
        DrawCircle(radius);
        StartCoroutine(WaitForExplosionCoroutine());

        
    }

    void DrawCircle(float r)
    {
        float localR = r / transform.lossyScale.x;
        float angleStep = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;
        for (int i = 0; i < subdivisions; i++)
        {
            float x = localR * Mathf.Cos(angleStep * i);
            float y = localR * Mathf.Sin(angleStep * i);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }


    private IEnumerator WaitForExplosionCoroutine()
    {
        float elapsed = 0f;
        float phase = 0f;
        float minSpeed = 2f;   // blinks per second at the start
        float maxSpeed = 15f;  // blinks per second at the end

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float speed = Mathf.Lerp(minSpeed, maxSpeed, t);
            phase += speed * Time.deltaTime;

            float blink = Mathf.Sin(phase * 2f * Mathf.PI) * 0.5f + 0.5f; // 0 to 1
            material.SetFloat(FlashAmountID, blink);


            // use it, e.g. on a color:
            // lineRenderer.startColor = lineRenderer.endColor = new Color(1f, 0f, 0f, blink);
            yield return null;
        }
        if(targetPlayer == false) CheckSurroundings();
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
        GameObject fx = Instantiate(effect, transform.position, transform.rotation);
        Destroy(fx, effectLifetime);
        Destroy(gameObject);
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
        Destroy(fx, effectLifetime);
        Destroy(gameObject);
    }
}