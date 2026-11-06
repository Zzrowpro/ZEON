
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AggroObstacle : Obstacle
{
    [Header("Aggro Settings")]
    [SerializeField]private float m_aggroSpeed = 7f;
    [SerializeField]private float rotationSpeed = 10f;

    [SerializeField] private Animator animator;
    private static readonly int[] StateHashes =
    {
    Animator.StringToHash("AggroObstacles_Blue"),
    Animator.StringToHash("AggroObstacles_Green"),
    Animator.StringToHash("AggroObstacles_Purple"),
    Animator.StringToHash("AggroObstacles_Red"),
    };

    private Transform target;
    private bool inRange;

    void Awake()
    {
         GameObject player = GameObject.FindWithTag("Player");
    if (player != null)
        target = player.transform;
    else
        CustomDebug.LogMessage("AggroObstacle: No GameObject with tag 'Player' found!");

    if (animator == null)
        animator = GetComponentInChildren<Animator>();
    }

    public override void Start()
    {
        base.Start();
        //Cant refrence the base start, this shi is hiding it FIX
        if (animator != null)
        animator.Play(StateHashes[num % StateHashes.Length], 0, Random.value);
    }

    protected override void HandleMovement()
    {
        if (inRange && target != null)
        {
            Vector2 targetPosition = target.position;
            Vector2 currentPosition = transform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            rb.MovePosition(currentPosition + (direction * m_aggroSpeed * Time.deltaTime));
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime) ;
            rb.MoveRotation(smoothedAngle);
        }
        else
        {
            base.HandleMovement();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        //CustomDebug.LogMessage("Halt");
        if (collision.gameObject.CompareTag("Player"))
            inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            inRange = false;
    }
}