using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Abilities : MonoBehaviour
{
   [SerializeField] private GameObject scarabPrefab;

    public InputActionAsset InputActions;

    private InputAction spawnScarap;
    private InputAction spawnBomb;

    private Rigidbody2D _rigibody2D;

   [Header("Scarab Settings")]
   [SerializeField] private int scarabCount = 2;
   [SerializeField] private float scarabCooldown = 5f;
   [SerializeField] private float scarabSpawnSpeed = 0.5f;
   [SerializeField] private Transform scarabSpawnPoint;
   [SerializeField] private bool canSpawnScarab = true;

   private float scarabCooldownTimer = 0f;

   [Header("Bombs")]
   [SerializeField]private GameObject bombPrefab;
   [SerializeField]private int totalBombs = 2;
   [SerializeField]private float bombCooldown = 4.0f;
   [SerializeField]private bool canSpawnBomb = true;

   private float bombCooldownTimer = 0f;

   private void Awake()
    {
        var map = InputActions.FindActionMap("Abilities");
        spawnScarap = map.FindAction("spawnScarab");
        spawnBomb = map.FindAction("spawnBomb");
        _rigibody2D = GetComponent<Rigidbody2D>();

    }

   void OnEnable()
    {
        InputActions.FindActionMap("Abilities").Enable();
    }
    void OnDisable()
    {
        InputActions.FindActionMap("Abilities").Disable();
    }

    void Update()
    {
        if(scarabCooldownTimer > 0f)
        {
            scarabCooldownTimer -= Time.deltaTime;
        }

        if (bombCooldownTimer > 0f)
        {
            bombCooldownTimer -= Time.deltaTime;
        }

        if (spawnScarap.WasPressedThisFrame() && CanSpawnScarab())
        {
            StartCoroutine(SpawnScarabCoroutine());
        }

        if (spawnBomb.WasPressedThisFrame() && totalBombs > 0 && CanSpawnBomb())
        {
            SpawnBomb();
        }
    }

    private void SpawnBomb()
    {
        bombCooldownTimer = bombCooldown;
        GameObject bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
        bomb.GetComponent<Rigidbody2D>().AddForce(bomb.transform.up * _rigibody2D.linearVelocity.magnitude, ForceMode2D.Impulse);
        totalBombs --;
    }
    private void SpawnScarab()
    {
       if (scarabPrefab == null)    
       {
           CustomDebug.LogMessage("Abilities: No scarab prefab assigned!");
           return;
       }
       Instantiate(scarabPrefab, scarabSpawnPoint.position, scarabSpawnPoint.rotation);
       CustomDebug.LogMessage("Scarab spawned!");
    }
    
    private bool CanSpawnBomb()
    {
        return bombCooldownTimer <= 0f && canSpawnBomb;
    }
    private bool CanSpawnScarab()
    {
        return scarabCooldownTimer <= 0f && canSpawnScarab;
    }

    private IEnumerator SpawnScarabCoroutine()
    {
        scarabCooldownTimer = scarabCooldown;
        for (int i = 0; i < scarabCount; i++)
        {    
            SpawnScarab();
            yield return new WaitForSeconds(scarabSpawnSpeed);
        }
       }
   }

