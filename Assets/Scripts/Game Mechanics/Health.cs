using Unity.Mathematics;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField]public int hp;
    [SerializeField]public int maxhp;
    public bool isDead = false;

    void Update()
    {
        if(hp > maxhp)
        {
            hp = maxhp;
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
            hp = math.clamp(hp - damage, 0, maxhp);

            if(hp <= 0)
            {
                Destroy(gameObject);
                isDead = true;
            }
        }
    }

    
}