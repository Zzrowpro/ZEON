using UnityEngine;
//Blueprint for abilities in Zeon, use when im making abilities so gonna have to wrewrite a lot of scripts
public abstract class Ability : MonoBehaviour
{
    public float cooldown = 2f;
    public float Cooldown => cooldown;
    public float CooldownRemaining => Mathf.Max(0f, nextReadyTime - Time.time);
    public float CooldownNormalized => cooldown > 0f ? CooldownRemaining / cooldown : 0f;
    
    public Sprite sprite;
    public Sprite DeactivatedSprite;
    private float nextReadyTime;
    public bool IsReady => Time.time >= nextReadyTime;

    public bool TryUse()
    {
        if (Time.time < nextReadyTime) return false;
        nextReadyTime = Time.time + cooldown; // start cooldown HERE, mistake in my former abilities scripts with cooldowns
        Activate();
        return true;
    }

    protected abstract void Activate();
}