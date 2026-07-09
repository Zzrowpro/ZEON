using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shooterobstacles : Obstacle
{
     [Header("Aggro Settings")]
    public float aggroSpeed = 7f;
    public float minRange = 0.5f;
   

    private Transform target;
    private bool inRange;
    public bool tryShoot = true;
    private bool isHalted = false;

    [Header("Shooter Settings")]
    [SerializeField]private GameObject projectilePrefab;
    [SerializeField]private int ammo;
    [SerializeField]private float nextFireTime;
    [SerializeField]private float shootingRate;

    public 

    void Awake() 
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            target = player.GetComponentInParent<Transform>();
        else
            Debug.LogWarning("AggroObstacle: No GameObject with tag 'Player' found!");
    }



    protected override void HandleMovement()
    {
        if (inRange && target != null)
        {
                Vector2 currentPosition = transform.position;
                Vector2 toTarget = (Vector2)target.position - currentPosition;
                float distance = toTarget.magnitude;
                if (distance > minRange)
                {
                    Vector2 direction = toTarget.normalized;
                    rb.MovePosition(currentPosition + (direction * aggroSpeed * Time.deltaTime));
                    Halt(true);
                }
        }
        else
        {
            base.HandleMovement();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Halt");
        if (collision.gameObject.CompareTag("Player"))
            inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            inRange = false;
            tryShoot = true;
            Halt(false);
    }

    private void ShootingStance()
    {
        Halt(true);
        
    }

    void Halt(bool halt)
    {
    isHalted = halt;
    if (halt)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // prevents physics from moving it
    }
    else
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    }

     private bool CanFire()
    {
        return ammo > 0 && Time.time >= nextFireTime;
    }

    private void Fire()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlasmaCanon: No projectile prefab assigned!");
            return;
        }

        ammo--;
        nextFireTime = Time.time + shootingRate;

        Instantiate(projectilePrefab, transform.position, transform.rotation);

        Debug.Log($"Fired! Ammo remaining: {ammo}");

        if (ammo <= 0)
        {
            Debug.Log("Out of ammo!");
            // OnOutOfAmmo?.Invoke(); // Uncomment if using an event
        }
    }

}