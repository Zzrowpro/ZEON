using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class PowerUp : MonoBehaviour
{

    [SerializeField] private float speed = 4f;
    [SerializeField] private float turnRate = 8f;
    [SerializeField] protected float lifetime = 5f;

    [Header("PickUpEffects")]
    [SerializeField] private GameObject pickupVFX;
    private float _effectsLifeTime;
    [SerializeField] private AudioClip pickUpSFX;

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
        AudioManager.instance.PlaySoundFXClip(pickUpSFX, transform, 1f);

        Apply(other.gameObject);
        GameObject vfx = Instantiate(pickupVFX,transform.position,Quaternion.identity);
        Destroy(vfx,3);
        Destroy(gameObject);
    }

    protected abstract void Apply(GameObject player);
}