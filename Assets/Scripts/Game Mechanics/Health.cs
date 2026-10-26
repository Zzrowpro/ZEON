using System;
using UnityEngine;

[RequireComponent(typeof(KnockBackFeedback))]
[RequireComponent(typeof(DamageFlash))]
public class Health : MonoBehaviour
{
    [SerializeField] private int hp = 1;
    [SerializeField] private int maxHp = 1;

    private bool isDead = false;
    private DamageFlash damageFlash;
    private KnockBackFeedback kbf;

    public event Action Died;

    void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        kbf = GetComponent<KnockBackFeedback>();
    }

    public void HpIncrease(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
    }

    public void TakeDamage(int damage, GameObject sender)
    {
        if (isDead) return;

        hp = Mathf.Clamp(hp - damage, 0, maxHp);
        damageFlash.CallDamageFlash();
        kbf.PlayerFeedback(sender);

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        isDead = true;
        Died?.Invoke();
        Destroy(gameObject);
    }
}