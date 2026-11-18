using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UFO : MonoBehaviour
{
    [SerializeField] private float [] laneOffsets = {-5f, -2.5f, 0f, 2.5f, 5f};
    [SerializeField] private Vector2 turnInterval = new Vector2(1f, 3f); // Time interval at which we change the direction in x axis 
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float speed = 0.5f;

    [SerializeField] private float laneSpeed = 2f;     
    [SerializeField] private float laneGain = 3f;       
    [SerializeField] private Vector2 laneChangeInterval = new Vector2(2f, 5f); //Time interval at which we change Y lanes

    private int targetLane;       
    private float nextLaneTime;   

    private float nextTurnTime;
    float direction = 1;
    
    private Rigidbody2D _rb;
    private Camera _mainCam;

    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        targetLane = Random.Range(0, laneOffsets.Length);
        _mainCam = Camera.main;
        ScheduleNextTurn();
        ScheduleNextLane();
    }

    void Update()
    {
        Vector3 viewportPos = _mainCam.WorldToViewportPoint(transform.position);
        bool onScreen = viewportPos.x > -0.1f && viewportPos.x < 1.1f;
        
        if (onScreen &&Time.time >= nextTurnTime)
            {
                direction = Random.value < 0.5f ? -1f : 1f;

                
                //direction = -direction;
                ScheduleNextTurn();
                
            }

        if(Time.time >= nextLaneTime)
        {
            int newLane;
            do { newLane = Random.Range(0, laneOffsets.Length); } while (newLane == targetLane);
            targetLane = newLane;
            ScheduleNextLane();
        } 

        if (viewportPos.x < 0f)      direction = 1f;
        else if (viewportPos.x > 1f) direction = -1f;  
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float laneTargetY = _mainCam.transform.position.y + laneOffsets[targetLane];
        float diff = laneTargetY - transform.position.y;    
        float targetX = direction * speed;
        float targetVelY = Mathf.Clamp(diff * laneGain, -laneSpeed, laneSpeed);
        
        
        

        float newX = Mathf.MoveTowards(_rb.linearVelocity.x, targetX, acceleration * Time.fixedDeltaTime);
        float newY = Mathf.MoveTowards(_rb.linearVelocity.y, targetVelY, acceleration * Time.fixedDeltaTime);
        _rb.linearVelocity = new Vector2(newX, newY);

        
        

    }

    private void ScheduleNextTurn()
    {
        nextTurnTime = Time.time + Random.Range(turnInterval.x, turnInterval.y);
    }

    private void ScheduleNextLane()
    {
        nextLaneTime = Time.time + Random.Range(laneChangeInterval.x, laneChangeInterval.y);
    }
}
