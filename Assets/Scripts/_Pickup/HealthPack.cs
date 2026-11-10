using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class HealthPack : MonoBehaviour
{
    [SerializeField]private int healthIncrease = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            player.HealthIncrease(healthIncrease);
            Destroy(gameObject);
        }
    }
}
