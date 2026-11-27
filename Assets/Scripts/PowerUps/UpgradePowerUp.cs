using UnityEngine;

public class UpgradePowerUp : PowerUp
{
    [SerializeField] float duration = 5f;
    [SerializeField] RocketProfiles newProfile;

    protected override void Apply(GameObject player)
    {
        if (player.TryGetComponent(out ProfileBuff buff))
            buff.Apply(newProfile, duration);
    }
}