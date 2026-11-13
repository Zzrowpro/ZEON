using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{

    //Actions 

    public InputActionAsset InputActions;

    //Input Actions
    private InputAction boost;
    private InputAction nitro;
    private InputAction mousePos;
    private InputAction aimStick;
    private InputAction turn;


    //Refrences
    [SerializeField]private Animator flameAnimator;
    
    [SerializeField]private FuelBar fuelBar;
    [SerializeField]private FuelBar nitroBar;
    [SerializeField]private GameObject nb;
    [SerializeField]public RocketProfiles rocketProfiles;

    [Header("Movement Abilities")]
    [SerializeField]private float fuel;
    public float Fuel
    {
        get => fuel;
        
        set => fuel = Mathf.Clamp(value, 0f, rocketProfiles.maxFuel);
        
    }
    [Header("Visuals")]
    [SerializeField]private GameObject boosterSprite;    

    private Rigidbody2D rb;
    private Vector2 mouseDirection;
    private bool boosting;
    private bool fuelCheck = false;

    [Header("Nitro")] // Stuff for the nitro ability
    [SerializeField] private float nitroFuel;
    public float NitroFuel
    {
        get => nitroFuel;
        set => nitroFuel = Mathf.Clamp(value, 0, rocketProfiles.maxNitroFuel);
    }
 
    private bool nitroOnCooldown = false;
    private bool isNitroBoosting = false;
    private float nitroCooldownTimer = 0f;

    [Header("Aiming")]
    [SerializeField] private float mouseSwitchThreshold = 5f;
    [SerializeField]private float stickDeadzone = 0.2f;
    [SerializeField]private float turnSpeed = 200f; //Turning with A & D on the keyboard
    
    private Vector2 aimDirection = Vector2.up;
    private float turnInput;
    private bool usingStick;
    private bool usingManual;

    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int BoostingHash = Animator.StringToHash("RocketBoost");

    //Pause System : 
    private PauseSystem pauseSystem;

    void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }


    void Awake()
    {
        //Pause System : 
        pauseSystem = FindFirstObjectByType<PauseSystem>();
        var map = InputActions.FindActionMap("Player");
        boost = map.FindAction("Boost");
        nitro = map.FindAction("Nitro"); // add this action in your Input Actions asset
        mousePos = map.FindAction("MousePos");
        aimStick = map.FindAction("AimStick");
        turn = map.FindAction("Turn", true);
        
    

        fuel = rocketProfiles.maxFuel;
        nitroFuel = rocketProfiles.maxNitroFuel;
        if(rocketProfiles.InfiniteMode == false)fuelBar.setMaxFuel(rocketProfiles.maxFuel);
        nitroBar.setMaxFuel(rocketProfiles.maxNitroFuel);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = rocketProfiles.linearDrag;
        rb.angularDamping = 5f;
    }

    void Update()
    {
        if(pauseSystem.GetIsPaused()) return;

        Vector2 stick = aimStick.ReadValue<Vector2>(); 
        bool stickActive = stick.sqrMagnitude > stickDeadzone * stickDeadzone;
        //bool mouseMoved = Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0f;
        bool mouseMoved = Mouse.current != null &&Mouse.current.delta.ReadValue().sqrMagnitude > mouseSwitchThreshold * mouseSwitchThreshold;
        turnInput = turn.ReadValue<float>();
        bool turnActive = Mathf.Abs(turnInput) > 0.01f;

        //Choosing what input type to use, might make this free handle where they all can trigger at the same time
        if (turnActive)
        {
            usingManual = true;     
        }
        else if (stickActive)
        {
            usingManual = false; usingStick = true;
        }
        else if (mouseMoved)
        {
            usingManual = false; usingStick = false; 
        }

        if (usingManual)
        {
            aimDirection = transform.up;
        }

        if(!usingManual)
        {
            if (usingStick)
            {
                if(stickActive) aimDirection = stick.normalized;
            }
            else
            {
                Vector2 screen = mousePos.ReadValue<Vector2>();
                Camera cam = Camera.main;

                // z = distance from camera to the plane the ship lives on
                float depth = Mathf.Abs(cam.transform.position.z - transform.position.z);
                Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(screen.x, screen.y, depth));

                aimDirection = ((Vector2)mouseWorld - (Vector2)transform.position).normalized;
            }
        }

        //Bar Displays
        fuelBar.setFuel(Fuel);
        nitroBar.setFuel(NitroFuel);

        // Mouse direction
        // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // mouseDirection = (mousePos - transform.position).normalized;

        if(rocketProfiles.InfiniteMode == false)
        {
            boosting = boost.IsPressed() && Fuel > 0;

            if (boosting && Fuel > 0)
            {
                boosterSprite.SetActive(true);
            }
            else if (!boosting)
            {
                boosterSprite.SetActive(false);
            }
        }
        else
        {
            boosting = boost.IsPressed();
        }
        

        //Nitro Mechanics
        isNitroBoosting = nitro.IsPressed() && nitroFuel > 0 && !nitroOnCooldown; //Where boost action is assigned

        if(flameAnimator.gameObject.activeSelf)
        flameAnimator.SetBool("isBoosting",isNitroBoosting);
        
        if (isNitroBoosting)
        {
            NitroFuel -= rocketProfiles.nitroLossRate * Time.deltaTime;
            nb.SetActive(true);
            
            if (nitroFuel <= 0)
                nitroOnCooldown = true;
        }
        else
        {
            NitroFuel += rocketProfiles.nitroRefreshRate * Time.deltaTime;
            nb.SetActive(true);
            if(nitroFuel == rocketProfiles.maxNitroFuel)
            {
                nb.SetActive(false);
            }
        }

        if (nitroOnCooldown)
        {
            nitroCooldownTimer += Time.deltaTime;
            if (nitroCooldownTimer >= rocketProfiles.nitroCooldownDuration)
            {
                nitroOnCooldown = false;
                nitroCooldownTimer = 0f;
            }
        }

    }

    void FixedUpdate()
    {
        if(Fuel > 0)
        {
            RotateShip();
            HandleThrust();
            KillSidewaysVelocity();
            ClampSpeed();
            //Burst();
            fuelCheck = false;
        }
        else if (Fuel == 0 && !fuelCheck) // Push the players spaceship in a random direction when game ends.
        {
            fuelCheck = true;
            Vector2 direction = UnityEngine.Random.insideUnitCircle; 
            rb.AddForce(direction * 5, ForceMode2D.Force);    
        }

    }

    // void RotateTowardsMouse() //MouseBased Movement
    // {
    //     float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
    //     float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
    //     rb.MoveRotation(smoothedAngle);
    // }

    void RotateShip()
    {
        if (usingManual)
        {
            if (Mathf.Abs(turnInput) > 0.01f)
                rb.MoveRotation(rb.rotation - turnInput * turnSpeed * Time.fixedDeltaTime);
            return; // hold heading, skip mouse/stick aiming
        }

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rocketProfiles.rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedAngle);
    }

    void HandleThrust() //Also handles nitro switching
    {
        if (!boosting) return;

        if(rocketProfiles.InfiniteMode == false)
        {
            rb.AddForce(transform.up * (isNitroBoosting ? rocketProfiles.acceleration * rocketProfiles.nitroMulti : rocketProfiles.acceleration));
            Fuel -= 0.1f;
        }
        else
        {
            rb.AddForce(transform.up *(isNitroBoosting ? rocketProfiles.acceleration * rocketProfiles.nitroMulti : rocketProfiles.acceleration));
        }
        
        
    }


    void KillSidewaysVelocity()
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;

        float forwardSpeed = Vector2.Dot(rb.linearVelocity, forward);
        float sidewaysSpeed = Vector2.Dot(rb.linearVelocity, right);

        rb.linearVelocity = forward * forwardSpeed + right * sidewaysSpeed * (1f - rocketProfiles.sidewaysDamping * Time.fixedDeltaTime);
    }

    void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > rocketProfiles.maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * rocketProfiles.maxSpeed;
        }
    }
 
    public void FuelIncrease(float value)
    {
        Fuel = math.round(fuel += value);
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