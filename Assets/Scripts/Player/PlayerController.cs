using System;
using Unity.VisualScripting;
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
    [SerializeField]private GameObject boosterSprite;    

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
    private bool isBoosting = false;
    private float nitroCooldownTimer = 0f;

    [Header("Aiming")]
    [SerializeField]private float stickDeadzone = 0.2f;
    [SerializeField]private float turnSpeed = 200f; //Turning with A & D on the keyboard
    
    private Vector2 aimDirection = Vector2.up;
    private float turnInput;
    private bool usingStick;
    private bool usingManual = false;

    void OnEnable()
    {
        InputActions.FindActionMap("Gameplay").Enable();
    }

    void OnDisable()
    {
        InputActions.FindActionMap("Gameplay").Disable();
    }


    void Awake()
    {
        var map = InputActions.FindActionMap("Gameplay");
        boost = map.FindAction("Boost");
        nitro = map.FindAction("Nitro"); // add this action in your Input Actions asset
        mousePos = map.FindAction("MousePos");
        aimStick = map.FindAction("AimStick");
        turn = map.FindAction("Turn", true);
        
    

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
        Vector2 stick = aimStick.ReadValue<Vector2>(); 
        bool stickActive = stick.sqrMagnitude > stickDeadzone * stickDeadzone;
        bool mouseMoved = Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0f;
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


        if(!usingManual)
        {
            if (usingStick)
            {
                if(stickActive) aimDirection = stick.normalized;
            }
            else
            {
                Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>());
                aimDirection = (mouseWorld - (Vector2)transform.position).normalized;
            }
        }

        //Bar Displays
        fuelBar.setFuel(fuel);
        nitroBar.setFuel(NitroFuel);

        // Mouse direction
        // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // mouseDirection = (mousePos - transform.position).normalized;


        boosting = boost.IsPressed() && fuel > 0;

        if (boosting && fuel > 0)
        {
            boosterSprite.SetActive(true);
        }
        else if (!boosting)
        {
            boosterSprite.SetActive(false);
        }

        //Nitro Mechanics
        isBoosting = nitro.IsPressed() && nitroFuel > 0 && !nitroOnCooldown; //Where boost action is assigned

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
            RotateShip();
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
            if(Math.Abs(turnInput) > 0.01f)
            {
                rb.MoveRotation(rb.rotation - turnInput * turnSpeed * Time.fixedDeltaTime);
                return;
            }
        }
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
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