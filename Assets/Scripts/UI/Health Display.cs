
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int hp;
    private int maxhp;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image [] hearts;

    public PlayerHealth health;

    void Update()
    {
        hp = health.hp;
        maxhp = health.maxhp; 
        HeartsDisplay(); 
    } 

    private void HeartsDisplay()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < hp)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxhp)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

}
