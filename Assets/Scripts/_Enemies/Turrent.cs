using UnityEngine;

public class Turrent : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]private float radius = 5f;
    [SerializeField]private float rotationSpeed = 5.0f;

    [Header("Circle")]
    [SerializeField]private LineRenderer lineRenderer;    
    [SerializeField]private int subdivisions = 10;
    [SerializeField] private int dashCount = 30;

    [Header("ShooterSettings")]
    [SerializeField]private GameObject bulletPrefab;
    [SerializeField]private Transform bulletSpawnPoint;
    [SerializeField]private float shootingRate = 0.5f;
    private float nextFireTime = 0f;
    
    private Transform m_target;
    private Collider2D [] m_hitcolliders = new Collider2D [34];
    
    void LateUpdate()
    {
        DrawCircle(radius);
    }
    void Awake()
    {
        lineRenderer.textureMode = LineTextureMode.Stretch;
        lineRenderer.material.mainTextureScale = new Vector2(dashCount, 1);
    }

    void Update()
    {
        if (m_target != null &&
        Vector2.Distance(transform.position, m_target.position) > radius)
        {
            m_target = null;
        }

        if (m_target == null)
        {
            SearchForPlayer();
        }
        else
        {
            Vector2 direction = m_target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if(CanFire() && bulletPrefab != null)
            {
                Fire();
            }
        }
    }

    void DrawCircle(float r)
    {
        float angleStep = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;
        Vector3 center = transform.position;
        for (int i = 0; i < subdivisions; i++)
        {
            float x = r * Mathf.Cos(angleStep * i);
            float y = r * Mathf.Sin(angleStep * i);
            lineRenderer.SetPosition(i, center + new Vector3(x, y, 0));
        }
    }

    private bool CanFire()
    {
        // return ammo > 0 && Time.time >= nextFireTime;
        return Time.time >= nextFireTime;
    }

    private void Fire()
    {
        nextFireTime = Time.time + shootingRate;
        if(bulletPrefab != null)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
    

    private void SearchForPlayer()
    {
        int numColliders = Physics2D.OverlapCircle(transform.position, radius, ContactFilter2D.noFilter, m_hitcolliders);
        for(int i = 0; i < numColliders; i++)
        {
            if (m_hitcolliders[i].CompareTag("Player") &&
            Vector2.Distance(transform.position, m_hitcolliders[i].transform.position) <= radius)
            {
                m_target = m_hitcolliders[i].transform;
                break;
            }
        }
    }
    //looks for player within its range 
}
