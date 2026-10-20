using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int Health;
    public int MaxHealth;
    [SerializeField]private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private Collider2D playerCollider;
    [SerializeField] private GameObject flames;
    private CinemachineImpulseSource impulseSource; //Screen shake

    private Color originalColor;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerController = gameObject.GetComponent<PlayerController>();
        impulseSource = gameObject.GetComponent<CinemachineImpulseSource>();
        playerCollider = GetComponent<Collider2D>(); 
        originalColor = spriteRenderer.color;

    }
    void Update()
    {
        if(Health > MaxHealth)
        {
            Health = MaxHealth;
        }

    }

    public void HealthIncrease(int amount)
    {
        Health += amount; 
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public void TakeDamage(int damage)
    {
        //Screenshake
        CameraShakeManager.instance.CameraShake(impulseSource);
        if (!isDead)
        {
            Health = math.clamp(Health - damage, 0, MaxHealth);
            TriggerFlash();

            if(Health <= 0)
            { 
                spriteRenderer.enabled = false;
                playerController.enabled = false;
                playerCollider.enabled = false;
                flames.SetActive(false);
                isDead = true;
            }
        }
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
    }

    
}