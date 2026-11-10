
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FuelPack : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    [SerializeField] private float fuelAmount;

    void Awake()
    {
        TryGetComponent(out circleCollider2D);
        circleCollider2D.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent(out PlayerController playerController);
            playerController.FuelIncrease(fuelAmount);
        }
    }
}