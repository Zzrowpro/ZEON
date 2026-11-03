using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProfiles", menuName = "Scriptable Objects/PlayerProfiles")]
public class RocketProfiles : ScriptableObject
{
    public int maxHealth = 5;

    public float shootingRate = 0.5f;
    [SerializeField] private GameObject bulletPrefabs;

    [Header("Movement Stats")]
    [SerializeField]private float acceleration = 15f;
    [SerializeField]private float maxSpeed = 8f;
    [SerializeField]private float rotationSpeed = 10f;
    [SerializeField]private float linearDrag = 1f;
    [SerializeField]private float sidewaysDamping = 3f;

    [Header("Fuel")]
    [SerializeField]private float maxFuel = 200f;
    [SerializeField] private float maxNitroFuel = 20f;

    [Header("Nitro")]
    [SerializeField] private float nitroMulti = 2f;
    [SerializeField] private float nitroLossRate = 1f;
    [SerializeField] private float nitroRefreshRate = 1f;
    [SerializeField] private float nitroCooldownDuration = 10f;  

    [Header("Aiming")]
    [SerializeField]private float stickDeadzone = 0.2f;
    [SerializeField]private float turnSpeed = 200f; //Turning with A & D on the keyboard
    
}
