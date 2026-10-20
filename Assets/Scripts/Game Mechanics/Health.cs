using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField]public int hp;
    [SerializeField]public int maxHp;
    public bool isDead = false;
    public GameObject effect;
    private GameObject activeEffect;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Update()
    {
        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }

    void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
       originalColor = spriteRenderer.color;
    }

    public void HpIncrease(int amount)
    {
        hp+= amount; 
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            hp = math.clamp(hp - damage, 0, maxHp);
            TriggerFlash();

            if(hp <= 0)
            {
                Dead();
            }
        }
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private void Dead()
    {
        if(effect != null)activeEffect = Instantiate(effect, transform.position, transform.rotation);
        Destroy(activeEffect, 2);
        isDead = true;
        Destroy(gameObject);
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
    }



    
}