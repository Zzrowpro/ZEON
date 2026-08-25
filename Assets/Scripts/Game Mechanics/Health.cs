using Unity.Mathematics;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField]public int hp;
    [SerializeField]public int maxHp;
    public bool isDead = false;

    void Update()
    {
        if(hp > maxHp)
        {
            hp = maxHp;
        }

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

            if(hp <= 0)
            {
                Destroy(gameObject);
                isDead = true;
            }
        }
    }

    
}