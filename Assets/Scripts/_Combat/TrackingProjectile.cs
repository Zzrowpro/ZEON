using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrackingProjectile : Projectile
{
    // [SerializeField] private Sprite notTargeting;
    // [SerializeField] private Sprite targeting;

    [Header("Targeting")]
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float rotateSpeed = 300f;

    private readonly Collider2D[] hits = new Collider2D[34];
    private ContactFilter2D filter;
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isTargeting;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        filter = new ContactFilter2D { useLayerMask = true, layerMask = targetMask, useTriggers = true };
    }


    void FixedUpdate()
    {
        if (target == null) FindClosestTarget();

        if (target != null)
        {
            Vector2 dir = ((Vector2)target.position - rb.position).normalized;
            float cross = Vector3.Cross(dir, transform.up).z;
            rb.angularVelocity = -cross * rotateSpeed;
            rb.linearVelocity = transform.up * bulletSpeed;
        }
        else
        {
            rb.angularVelocity = 0f;
        }

        // SetTargeting(target != null);
    }

    void FindClosestTarget()
    {
        int n = Physics2D.OverlapCircle(transform.position, radius, filter, hits);
        float best = float.MaxValue;
        for (int i = 0; i < n; i++)
        {
            float d = ((Vector2)hits[i].transform.position - rb.position).sqrMagnitude;
            if (d < best) { best = d; target = hits[i].transform; }
        }
    }

    // void SetTargeting(bool value)
    // {
    //     if (isTargeting == value) return;
    //     isTargeting = value;
    //     sr.sprite = value ? targeting : notTargeting;
    // }
}