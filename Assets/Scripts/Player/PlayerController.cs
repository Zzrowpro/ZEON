using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField]private FuelBar fuelBar;
    [SerializeField]private FuelBar nitroBar;
    [SerializeField]private GameObject nb;

    [Header("Movement")]
    [SerializeField]private float acceleration = 15f;
    [SerializeField]private float maxSpeed = 8f;
    [SerializeField]private float rotationSpeed = 10f;
    [SerializeField]private float linearDrag = 1f;
    [SerializeField]private float sidewaysDamping = 3f;

    [Header("Movement Abilities")]
    [SerializeField]private float maxFuel = 200f;
    [SerializeField]private float fuel;
    public float Fuel
    {
        get => fuel;
        
        set => fuel = Mathf.Clamp(value, 0f, maxFuel);
        
    }

    [Header("Visuals")]
    public GameObject boosterSprite;    

    private Rigidbody2D rb;
    private Vector2 mouseDirection;
    private bool boosting;

    private bool fuelCheck = false;

    [Header("Nitro")] // Stuff for the nitro ability
    [SerializeField] private float maxNitroFuel = 20f;
    [SerializeField] private float nitroFuel;
    public float NitroFuel
    {
        get => nitroFuel;
        set => nitroFuel = Mathf.Clamp(value, 0, maxNitroFuel);
    }

    [SerializeField] private float nitroMulti = 2f;
    [SerializeField] private float nitroLossRate = 1f;
    [SerializeField] private float nitroRefreshRate = 1f;
    [SerializeField] private float nitroCooldownDuration = 10f;  
    private bool nitroOnCooldown = false;
    private bool canBoost = true;
    private bool isBoosting = false;
    private float nitroCooldownTimer = 0f;


    void Awake()
    {
        fuel = maxFuel;
        nitroFuel = maxNitroFuel;
        fuelBar.setMaxFuel(maxFuel);
        nitroBar.setMaxFuel(maxNitroFuel);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = linearDrag;
        rb.angularDamping = 5f;
    }

    void Update()
    {
        //Bar Displays
        fuelBar.setFuel(fuel);
        nitroBar.setFuel(NitroFuel);
        
        boosting = Mouse.current.leftButton.isPressed;

        // Mouse direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mouseDirection = (mousePos - transform.position).normalized;

        if (boosting && fuel > 0)
        {
            boosterSprite.SetActive(true);
        }
        else if (!boosting)
        {
            boosterSprite.SetActive(false);
        }

        //Nitro Mechanics
        isBoosting = Input.GetButton("Boost") && nitroFuel > 0 && !nitroOnCooldown;

        if(animator.gameObject.activeSelf)
        animator.SetBool("isBoosting",isBoosting);
        
        if (isBoosting)
        {
            NitroFuel -= nitroLossRate * Time.deltaTime;
            nb.SetActive(true);
            
            if (nitroFuel <= 0)
                nitroOnCooldown = true;
        }
        else
        {
            NitroFuel += nitroRefreshRate * Time.deltaTime;
            nb.SetActive(true);
            if(nitroFuel == maxNitroFuel)
            {
                nb.SetActive(false);
            }
        }

        if (nitroOnCooldown)
        {
            nitroCooldownTimer += Time.deltaTime;
            if (nitroCooldownTimer >= nitroCooldownDuration)
            {
                nitroOnCooldown = false;
                nitroCooldownTimer = 0f;
            }
        }

    }

    void FixedUpdate()
    {
        if(fuel > 0)
        {
            RotateTowardsMouse();
            HandleThrust();
            KillSidewaysVelocity();
            ClampSpeed();
            //Burst();
            fuelCheck = false;
        }
        else if (fuel == 0 && !fuelCheck) // Push the players spaceship in a random direction when game ends.
        {
            fuelCheck = true;
            Vector2 direction = UnityEngine.Random.insideUnitCircle; 
            rb.AddForce(direction * 5, ForceMode2D.Force);    
        }

    }

    void RotateTowardsMouse() //MouseBased Movement
    {
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedAngle);
    }

    void HandleThrust() //Also handles nitro switching
    {
        if (!boosting) return;

        rb.AddForce(transform.up * (isBoosting ? acceleration * nitroMulti : acceleration));
        fuel -= 0.1f;
        
    }

    void KillSidewaysVelocity()
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;

        float forwardSpeed = Vector2.Dot(rb.linearVelocity, forward);
        float sidewaysSpeed = Vector2.Dot(rb.linearVelocity, right);

        rb.linearVelocity = forward * forwardSpeed + right * sidewaysSpeed * (1f - sidewaysDamping * Time.fixedDeltaTime);
    }

    void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    // void Burst() // TS DOES NOT WORK BTW
    // {
    //     if (Keyboard.current.spaceKey.wasPressedThisFrame)
    //     {
    //         Debug.Log("Is ts working");
    //         rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    //     }
    //     else
    //     {
    //         return;
    //     }
    // }
}