using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class HealthPack : MonoBehaviour
{
    [SerializeField]private int hpInc;
    [SerializeField]private float acceleration = 2f;
    [SerializeField]private float maxSpeed = 5f;


    private float lifespan = 5f;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health player = collision.gameObject.GetComponent<Health>();
            player.HpIncrease(hpInc);
        }
    }
}
