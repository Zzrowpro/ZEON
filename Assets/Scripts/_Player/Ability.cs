using UnityEngine;
//Blueprint for abilities in Zeon, use when im making abilities so gonna have to wrewrite a lot of scripts
public abstract class Ability : MonoBehaviour
{
    [SerializeField] float cooldown = 2f;
    float nextReadyTime;

    public bool TryUse()
    {
        if (Time.time < nextReadyTime) return false;
        nextReadyTime = Time.time + cooldown; // start cooldown HERE, mistake in my former abilities scripts with cooldowns
        Activate();
        return true;
    }

    protected abstract void Activate();
}