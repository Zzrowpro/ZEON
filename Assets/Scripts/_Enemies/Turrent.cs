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

    [Header("Prediction")] 
    [SerializeField] private float bulletSpeed = 10f; 
    [Range(0f, 1f)]
    [SerializeField] private float accuracy = 1f; 
    [SerializeField] private float maxSpreadDegrees = 15f; 
    [SerializeField] private float velocitySmoothing = 10f; 
    [SerializeField] private float fireAngleTolerance = 5f; 

    private Transform m_target;
    private Collider2D [] m_hitcolliders = new Collider2D [34];
    private Vector2 m_lastTargetPos; 
    private Vector2 m_targetVelocity; 

    private Vector3 ShotOrigin => bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position; 

    void LateUpdate()
    {
        DrawCircle(radius);
    }
    void Awake()
    {
        lineRenderer.textureMode = LineTextureMode.Stretch;
        lineRenderer.material.mainTextureScale = new Vector2(dashCount, 1);
        bulletSpeed = bulletPrefab.GetComponent<EnemyProjectile>().bulletSpeed;
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
            TrackTargetVelocity(); 
            Vector2 aimPoint = GetAimPoint(); 
            Vector2 direction = aimPoint - (Vector2)transform.position; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (CanFire() && bulletPrefab != null &&
                Vector2.Angle(transform.up, direction) <= fireAngleTolerance) 
            {
                Fire();
            }
        }
    }

  
    private void TrackTargetVelocity()
    {
        if (Time.deltaTime <= 0f) return;
        Vector2 pos = m_target.position;
        Vector2 rawVelocity = (pos - m_lastTargetPos) / Time.deltaTime;
        float t = 1f - Mathf.Exp(-velocitySmoothing * Time.deltaTime);
        m_targetVelocity = Vector2.Lerp(m_targetVelocity, rawVelocity, t);
        m_lastTargetPos = pos;
    }

   
    private Vector2 GetAimPoint()
    {
        Vector2 targetPos = m_target.position;
        Vector2 predicted = targetPos;

        if (TryGetInterceptTime((Vector2)(targetPos - (Vector2)ShotOrigin), m_targetVelocity, bulletSpeed, out float t))
        {
            predicted = targetPos + m_targetVelocity * t;
        }
        return Vector2.Lerp(targetPos, predicted, accuracy);
    }

    
    private static bool TryGetInterceptTime(Vector2 d, Vector2 v, float s, out float time)
    {
        time = 0f;
        float a = Vector2.Dot(v, v) - s * s;
        float b = 2f * Vector2.Dot(d, v);
        float c = Vector2.Dot(d, d);

        if (Mathf.Abs(a) < 0.0001f)
        {
            if (Mathf.Abs(b) < 0.0001f) return false;
            time = -c / b;
            return time > 0f;
        }

        float disc = b * b - 4f * a * c;
        if (disc < 0f) return false;

        float sqrt = Mathf.Sqrt(disc);
        float t1 = (-b - sqrt) / (2f * a);
        float t2 = (-b + sqrt) / (2f * a);

        if (t1 > 0f && t2 > 0f) time = Mathf.Min(t1, t2);
        else if (t1 > 0f) time = t1;
        else if (t2 > 0f) time = t2;
        else return false;
        return true;
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
        return Time.time >= nextFireTime;
    }

    private void Fire()
    {
        nextFireTime = Time.time + shootingRate;
        if(bulletPrefab != null)
        {
            float spread = Random.Range(-1f, 1f) * (1f - accuracy) * maxSpreadDegrees; 
            Quaternion rot = transform.rotation * Quaternion.Euler(0, 0, spread); 
            Instantiate(bulletPrefab, ShotOrigin, rot); 
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
                m_lastTargetPos = m_target.position; 
                m_targetVelocity = Vector2.zero; 
                break;
            }
        }
    }
}