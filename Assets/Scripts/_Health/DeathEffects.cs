using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectLifetime = 2f;
    [SerializeField] private bool scaleWithObject = false;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()  { health.Died += OnDied; }
    void OnDisable() { health.Died -= OnDied; }

    private void OnDied()
    {
        HandleParticles();
    }

    private void HandleParticles()
    {
        if(scaleWithObject == true)
        {
            GameObject fx = Instantiate(effectPrefab, transform.position, transform.rotation);
            fx.transform.localScale = new Vector3(transform.localScale.x, transform.lossyScale.y, 0);
            Destroy(fx, effectLifetime);
        }
        else
        {
            GameObject fx = Instantiate(effectPrefab, transform.position, transform.rotation);
            Destroy(fx, effectLifetime);
        }
    }
}