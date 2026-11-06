using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int health = 4;
    public int Health
    {
        get{return health;}
        set
        {
            health = Math.Clamp(value, 0, rocketProfiles.maxHealth);
            if(health <= 0)
            {
                OnPlayerDeath?.Invoke();
                isDead = true;
                spriteRenderer.enabled = false;
                playerController.enabled = false;
                playerCollider.enabled = false;
                abilities.enabled = false;
                flames.SetActive(false);
            }
        }
    }
    [SerializeField]private bool isDead = false;
    [SerializeField] RocketProfiles rocketProfiles;
    [SerializeField] private GameObject flames;

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private Collider2D playerCollider;
    private Abilities abilities;
    

    private CinemachineImpulseSource impulseSource; //Screen shake´

    private DamageFlash damageFlash;

    public event Action OnPlayerDeath;


    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerController = gameObject.GetComponent<PlayerController>();
        impulseSource = gameObject.GetComponent<CinemachineImpulseSource>();
        playerCollider = GetComponent<Collider2D>();
        damageFlash = GetComponent<DamageFlash>();
        abilities = GetComponent<Abilities>();
    }
    public void HealthIncrease(int amount)
    {
        Health += amount; 
    }

 

    public void TakeDamage(int damage)
    {
        //Screenshake
        CameraShakeManager.instance.CameraShake(impulseSource);
        if (!isDead)
        {
            Health = math.clamp(Health - damage, 0, rocketProfiles.maxHealth);
            damageFlash.CallDamageFlash();

        }
    }

    
}