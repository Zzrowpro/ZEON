using Unity.Mathematics;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField]public int hp;
    [SerializeField]public int maxhp;
    public bool isDead = false;

    private SpriteRenderer sr;
    private PlayerController pc;

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        pc = gameObject.GetComponent<PlayerController>();
    }
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
                sr.enabled = false;
                pc.enabled = false;
                isDead = true;
            }
        }
    }

    
}