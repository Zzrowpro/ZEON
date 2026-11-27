using UnityEngine;

public class NitroRefuelPickup : PowerUp
{
    [SerializeField] private float increaseAmount = 1f;
    protected override void Apply(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>(); 
        playerController.NitroIncrease(increaseAmount);
    }
}