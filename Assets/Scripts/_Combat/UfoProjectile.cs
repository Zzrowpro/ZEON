using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UfoProjectile : EnemyProjectile
{
    [SerializeField]private float trackDuration = 0.5f; 

    private Transform _target;
    private Transform _playerTransform;

    private Rigidbody2D _rb;

    
    
    
    
    void Awake()
    {
        _playerTransform = GameObject.Find("Player").transform; 
        _target = _playerTransform;
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        StartCoroutine(TrackingCoroutine());
        Destroy(gameObject, lifetime);
    }


    private IEnumerator TrackingCoroutine()
    {
        float elapsedTime = 0f;
        Vector2 lastDir = transform.right;
        while(elapsedTime < trackDuration)
        {
            elapsedTime += Time.deltaTime;
            Vector2 toTarget = (Vector2)(_target.position - transform.position);
            lastDir = toTarget.normalized;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, bulletSpeed * Time.deltaTime);
            yield return null;
        }
        _rb.linearVelocity = lastDir * bulletSpeed;
        
    }


}