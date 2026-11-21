using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float turnRate = 8f;
    [SerializeField] protected float lifetime = 5f;

    [Header("PickUpEffects")]
    [SerializeField] private GameObject _effects;
    private float _effectsLifeTime;

    private float id;
    private float angle;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _effectsLifeTime = lifetime - 1;
        id = Random.Range(0f, 100f);
        angle = Random.Range(0f, Mathf.PI * 2f);    
    }

    void Update()
    {
        angle += (Mathf.PerlinNoise(Time.time * 0.8f, id) - 0.5f) * turnRate * Time.deltaTime;
        transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Apply(other.gameObject);
        if(_effects != null)
        {
            GameObject fx = Instantiate(_effects, transform.position, transform.rotation);
            Destroy(fx, _effectsLifeTime);
        }
        Destroy(gameObject);
    }

    protected abstract void Apply(GameObject player);
}