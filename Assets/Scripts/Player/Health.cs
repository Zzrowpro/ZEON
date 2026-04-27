using UnityEngine;

public class Health : MonoBehaviour
{
    public int hp;
    public int maxhp;

    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }

        if(hp > maxhp)
        {
            hp = maxhp;
        }
    }

    public void HpIncrease(int amount)
    {
        hp+= amount; 
    }
}