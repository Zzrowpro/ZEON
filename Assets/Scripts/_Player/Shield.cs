using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Health))]
public class Shield : MonoBehaviour
{
    // Animator state hashes. Index -1 (full health) uses ForceFieldHash.
    private static readonly int ForceFieldHash = Animator.StringToHash("ForceField");
    private static readonly int[] StateHashes =
    {
        Animator.StringToHash("Stage_1_Forcefield"),
        Animator.StringToHash("Stage_2_Forcefield"),
        Animator.StringToHash("Stage_3_Forcefield"),
    };

    [Header("Combat")]
    [SerializeField] private float knockBack = 1f;
    [SerializeField] private int damage = 2;

    [Header("Visuals")]
    [Tooltip("Only used if there is no Animator on this object.")]
    [SerializeField] private Sprite[] shieldSprites;
    [SerializeField] private Sprite fullHealthSprite;
    [SerializeField] private Animator animator;

    [Header("Debug (read-only)")]
    [SerializeField] private int stage = -1;

    private Transform player;
    private Health shieldHealth;
    private SpriteRenderer sr;
    private int currentStage = -2; // -2 = nothing applied yet, forces first update

    void Awake()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogError("Shield: no GameObject named 'Player' found.", this);

        shieldHealth = GetComponent<Health>();
        sr = GetComponent<SpriteRenderer>();

        // Don't overwrite an Animator assigned in the Inspector
        if (animator == null) animator = GetComponent<Animator>();
    }

    // void OnEnable()
    // {
    //     shieldHealth.TookDamage += OnTookDamage;
    // }

    // void OnDisable()
    // {
    //     shieldHealth.TookDamage -= OnTookDamage;
    // }

    // void Start()
    // {
    //     currentStage = -2;
    //     ChangeSpriteBasedOnHealth(); // set the correct look at the start
    // }

    // private void OnTookDamage()
    // {
    //     ChangeSpriteBasedOnHealth();
    // }

    void Update()
    {
        if (player == null) return;

        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0f, 0f, player.eulerAngles.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (health != null) health.TakeDamage(damage, gameObject);
            shieldHealth.TakeDamage(1, collision.gameObject);
            if (rb != null) rb.AddForce(transform.up * knockBack, ForceMode2D.Impulse);
        }
    }

    // public void ChangeSpriteBasedOnHealth()
    // {
    //     // Cast to float so we don't do integer division
    //     percent = (float)shieldHealth.Hp / shieldHealth.MaxHp;

    //     if (percent > 0.75f)      stage = -1; // healthy
    //     else if (percent > 0.50f) stage = 0;
    //     else if (percent > 0.25f) stage = 1;
    //     else                      stage = 2;

    //     // Only react when the stage actually changes
    //     if (stage == currentStage) return;
    //     currentStage = stage;

    //     if (animator != null)
    //     {
    //         int hash = (stage == -1) ? ForceFieldHash : StateHashes[stage];

    //         if (!animator.HasState(0, hash))
    //         {
    //             Debug.LogError($"Shield: animator has NO state for stage {stage} (hash {hash}). " +
    //                         $"Check the state name and that this Animator uses the right controller.", this);
    //             return;
    //         }

    //         Debug.Log($"Shield: playing stage {stage}, hp% = {percent:F2}", this);
    //         animator.Play(hash, 0, 0f);
    //     }
    //     else if (sr != null)
    //     {
    //         // No animator, so swap sprites manually
    //         if (stage == -1) sr.sprite = fullHealthSprite;
    //         else if (shieldSprites != null && stage < shieldSprites.Length)
    //             sr.sprite = shieldSprites[stage];
    //     }
    // }
}