using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shooterobstacles : Obstacle, IMimicable //This is an interface it creates a blueprint for the requirements of a class
{
     [Header("Aggro Settings")]
    public float aggroSpeed = 7f;
    public float minRange = 0.5f;
    public float buffer = 0.5f; //This is a buffer helps when fluctuating between 2 values
   

    public Transform target;
    private PlayerController playerController;
    private bool inRange;
    public bool tryShoot = true;
    private bool isHalted = false;

    private GameObject activeProjectile;

    [SerializeField]private GameObject Halo;

    [Header("Shooter Settings")]
    [SerializeField]private GameObject projectilePrefab;
    private float nextFireTime;
    [SerializeField]private float shootingRate;
 
    void Awake() 
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            target = player.GetComponentInParent<Transform>();
        else
            Debug.LogWarning("AggroObstacle: No GameObject with tag 'Player' found!");

            playerController = player.GetComponent<PlayerController>();

    }
    protected override void HandleMovement()
    {
        if (inRange && target != null)
        {
            Vector2 currentPosition = transform.position;
            Vector2 toTarget = (Vector2)target.position - currentPosition;
            float distance = toTarget.magnitude;
            if (distance > minRange + buffer)
            {
                Vector2 direction = toTarget.normalized;
                rb.MovePosition(currentPosition + (direction * aggroSpeed * Time.deltaTime));
                ShootingStance();
            }
            else if( distance < minRange - buffer && distance > 0 )
            {
                Vector2 direction = ((Vector2)transform.position - (Vector2)target.position).normalized;
                ///transform.position += (Vector3)(direction * aggroSpeed * Time.deltaTime);
                rb.MovePosition(currentPosition + (direction * aggroSpeed * Time.deltaTime)); 
                ShootingStance();
            }
            else
            {
                ShootingStance();
            }
              
        }
        else if(inRange == false)
        {
            Halt(false);
            Debug.Log("Back to normal");
            base.HandleMovement();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Halt");
        if (collision.gameObject.CompareTag("Player"))
            inRange = true;
            Debug.Log("In range");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            tryShoot = true;
            Debug.Log("Not inrange");
        }
    }
            

    private void ShootingStance()
    {
        Halt(true);
        if (CanFire())
        {
            Fire();
        }
        
    }

    void Halt(bool halt)
    {
    isHalted = halt;
        if (halt)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; // prevents physics from moving it
        }
        else if(halt == false)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void CopyStateFrom(IMimicable other)
    {
        
    }

    private bool CanFire()
    {
        return Time.time >= nextFireTime && CheckForProjectileGone();
    }

    private bool CheckForProjectileGone()
    {
        bool gone = activeProjectile == null; // Unity overloads == so this is true after Destroy() too
        Halo.SetActive(!gone); //Can keep track of instantiated prefabs, good to note. 
        return gone;
    }

    private void Fire()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlasmaCanon: No projectile prefab assigned!");
            return;
        }
        nextFireTime = Time.time + shootingRate;
        activeProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        IMimicable bMimic = activeProjectile.GetComponent<IMimicable>();
        bMimic.CopyStateFrom(this);
    }

}