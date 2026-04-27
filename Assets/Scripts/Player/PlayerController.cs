using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float acceleration = 15f;
    public float maxSpeed = 8f;
    public float rotationSpeed = 10f;
    public float linearDrag = 1f;
    public float sidewaysDamping = 3f;
    public float fuel = 200f;

    [Header("Visuals")]
    public GameObject boosterSprite;    

    private Rigidbody2D rb;
    private Vector2 mouseDirection;
    private bool boosting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = linearDrag;
        rb.angularDamping = 5f;
    }

    void Update()
    {
        boosting = Mouse.current.leftButton.isPressed;

        // Mouse direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mouseDirection = (mousePos - transform.position).normalized;

        if (boosting)
        {
            boosterSprite.SetActive(true);
        }
        else if (!boosting)
        {
            boosterSprite.SetActive(false);
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
        }
    }

    void RotateTowardsMouse()
    {
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedAngle);
    }

    void HandleThrust()
    {
        if (!boosting) return;

        rb.AddForce(transform.up * acceleration);
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
}