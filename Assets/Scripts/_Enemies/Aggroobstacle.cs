

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AggroObstacle : Obstacle
{
    [Header("Aggro Settings")]
    [SerializeField]private float rangeRadius = 2f;
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

    private Transform m_target;
    private Transform playerTransform;
    private bool inRange;
    private Collider2D [] m_hitcolliders = new Collider2D[34];

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }        
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

    void Update()
    {
         SearchForTarget();
    }

    protected override void HandleMovement()
    {
        if (inRange && m_target != null)
        {
            Vector2 targetPosition = m_target.position;
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


    public float GetScaledRadius()
    {
        float maxRadius = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        return rangeRadius * maxRadius;
    }

    private void SearchForTarget()
    {
        int count = Physics2D.OverlapCircle(transform.position, GetScaledRadius(), ContactFilter2D.noFilter, m_hitcolliders);
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            if (!m_hitcolliders[i].TryGetComponent(out Decoy decoy)) continue;

            float dist = Vector2.Distance(transform.position, decoy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = decoy.transform;
            }
        }

        if (closest != null)
        {
            m_target = closest;
            inRange = true;
        }
        else if (playerTransform != null)
        {
            m_target = playerTransform;
            inRange = Vector2.Distance(transform.position, playerTransform.position) <= GetScaledRadius();
        }
        else
        {
            m_target = null;
            inRange = false;
        }
    }
}