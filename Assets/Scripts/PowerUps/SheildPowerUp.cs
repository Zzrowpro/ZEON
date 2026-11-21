using UnityEngine;

public class SheildPowerUp : PowerUp
{
    [SerializeField]private GameObject sheildPrefab;

    protected override void Apply(GameObject player)
    {
        Transform playerTransform = player.transform;
        Instantiate(sheildPrefab, playerTransform.position, playerTransform.rotation);
    }
}