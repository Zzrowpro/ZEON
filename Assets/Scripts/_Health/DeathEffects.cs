using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectLifetime = 2f;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()  { health.Died += OnDied; }
    void OnDisable() { health.Died -= OnDied; }

    private void OnDied()
    {
        GameObject fx = Instantiate(effectPrefab, transform.position, transform.rotation);
        Destroy(fx, effectLifetime);
    }
}