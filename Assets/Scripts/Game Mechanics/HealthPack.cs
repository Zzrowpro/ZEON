using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class HealthPack : MonoBehaviour
{
    public int hpInc;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health player = collision.gameObject.GetComponent<Health>();
            player.HpIncrease(hpInc);
        }
    }
}
