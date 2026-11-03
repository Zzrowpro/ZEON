using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProfiles", menuName = "Scriptable Objects/PlayerProfiles")]
public class RocketProfiles : ScriptableObject
{
    public int maxHealth = 5;

    public float shootingRate = 0.5f;
    public GameObject bulletPrefabs;

    [Header("Movement Stats")]
    public float acceleration = 15f;
    public float maxSpeed = 8f;
    public float rotationSpeed = 10f;
    public float linearDrag = 1f;
    public float sidewaysDamping = 3f;

    [Header("Fuel")]
    public float maxFuel = 200f;
    public float maxNitroFuel = 20f;

    [Header("Nitro")]
    public float nitroMulti = 2f;
    public float nitroLossRate = 1f;
    public float nitroRefreshRate = 1f;
    public float nitroCooldownDuration = 10f;  

    [Header("Aiming")]
    public float stickDeadzone = 0.2f;
    public float turnSpeed = 200f; //Turning with A & D on the keyboard
    
}
