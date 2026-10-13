
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int health;
    private int maxHealth;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image [] hearts;

    public PlayerHealth playerHealth;

    void Update()
    {
        health = playerHealth.Health;
        maxHealth = playerHealth.MaxHealth; 
        HeartsDisplay(); 
    } 

    private void HeartsDisplay()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
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
