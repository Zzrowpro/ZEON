using UnityEngine;

[RequireComponent(typeof(Health))]
public class PointsReward : MonoBehaviour
{
    [SerializeField] private int points = 1;
    [SerializeField] private GameObject popupPrefab;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()  { health.Died += OnDied; }
    void OnDisable() { health.Died -= OnDied; }

    private void OnDied()
    {
        DataManager.instance.points += points;

        GameObject obj = Instantiate(popupPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<Popup>().points = points;
    }
}