using UnityEngine;

public class TrackingPowerUp : PowerUp
{
    [Header("PowerUp Settings")]
    [SerializeField] private float effectDuration = 5f;
    private PlasmaCanon plasmaCanon;

    protected override void Apply(GameObject player)
    {
        plasmaCanon = player.GetComponent<PlasmaCanon>();
        plasmaCanon.ApplyBulletType(BulletType.Homing, effectDuration);
    }
}