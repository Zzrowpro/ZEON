using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UFO_Canon : MonoBehaviour
{
    //Always able to shoot player as long as in scene
    //FireRate
    //Damage

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform nossle;
    [SerializeField] private Transform [] instatiatePoints;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float rotationSpeed = 1f; //If i want it to have a nossle like the turrent 

     [Header("VFX")]
    [SerializeField] private GameObject shootVfx;
    [SerializeField] private float effectLifetime;
    [SerializeField] private Vector3 shootVfxScale = new Vector3(1,1,1);

    private Rigidbody2D _rb;
    private Camera _mainCam;
    private Transform _playerTransform;
    private Transform _target;
    private float _nextFireTime;

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        _playerTransform = player.transform;
        _mainCam = Camera.main;
    
        _target = _playerTransform;
    }

    void Start()
    {
        GetNextFireTime();
    }

    void Update()
    { 
        Vector3 viewportPos = _mainCam.WorldToViewportPoint(transform.position);
        bool onScreen = viewportPos.x > 0f && viewportPos.x < 1f && viewportPos.y > 0f && viewportPos.y < 1f;
        
        
        if(Time.time >= _nextFireTime && onScreen)
        {
            if(shootVfx != null)
            {
                GameObject sVfx =  Instantiate(shootVfx, transform.position, transform.rotation);
                sVfx.transform.localScale = shootVfxScale;
                Destroy(sVfx, effectLifetime);
            }
            foreach(Transform transform in instatiatePoints)
            {
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
            GetNextFireTime();
        }
    }
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)_target.position -(Vector2)nossle.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        nossle.rotation = Quaternion.Slerp(nossle.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void GetNextFireTime()
    {
        _nextFireTime = Time.time + fireRate; 
    }
}
