 using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(KnockBackFeedback))]
[RequireComponent(typeof(DamageFlash))]
public class Health : MonoBehaviour
{
    [SerializeField] private string id = "id";
    [SerializeField] private int hp = 1;
    [SerializeField] private GameObject deathVFX;
    
    public int Hp
    {
        get{return hp;}
        private set
        {
            hp = math.clamp(value, 0, maxHp); 
            if(hp<= 0)
            {
                Dead();
            }
        }
    }

    [SerializeField] private int maxHp = 1;

    public int MaxHp
    {
        get{return maxHp;}
        private set{}
    }
    

    private bool isDead = false;
    private DamageFlash damageFlash;
    private KnockBackFeedback kbf;

    public event Action Died;
    public event Action TookDamage;

    void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        kbf = GetComponent<KnockBackFeedback>();
    }

    public void HpIncrease(int amount)
    {
        Hp = Mathf.Min(Hp + amount, maxHp);
    }

    public void TakeDamage(int damage, GameObject sender)
    {
        if (isDead) return;
        TookDamage?.Invoke();
        Hp -= damage;
        damageFlash.CallDamageFlash();
        kbf.PlayerFeedback(sender);
    }

    public void TakeDamage(int damage)
    {
        if(isDead) return;
        TookDamage?.Invoke();
        Hp -= damage;
        damageFlash.CallDamageFlash();
    }

    private void Dead()
    {
        isDead = true;
        GameEvents.Report(id);
        kbf.Cancel();
        Died?.Invoke();
        Destroy(gameObject);

        if (deathVFX != null)
            Destroy(Instantiate(deathVFX,transform.position,Quaternion.identity),4);
    }
}