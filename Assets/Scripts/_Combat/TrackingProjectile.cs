using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrackingProjectile : Projectile
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform target;
    [SerializeField] private Sprite notTargeting;
    [SerializeField]private Sprite targeting;
    [SerializeField] private LayerMask targetLayer;
    private bool isTargeting;

    [Header("Configurable values")]
    private int rotateSpeed;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void  Start()
    {
        base.Start();
        Raycast(); 
    }

    void Update()
    {
        if (isTargeting)
        {
            spriteRenderer.sprite = targeting;
        }
        else
        {
            spriteRenderer.sprite = notTargeting;
        }
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.linearVelocity = transform.up * bulletSpeed;
            
            isTargeting = true;
        }
        if(target == null)
        {
            isTargeting = false;
        }
    }

    private void Raycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, targetLayer);
        if (hit)
        {
            target = hit.transform;
        }
    }
}
