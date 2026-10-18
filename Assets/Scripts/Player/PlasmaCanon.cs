using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class PlasmaCanon : MonoBehaviour
{

    //Actions
    public InputActionAsset InputActions;

    private InputAction fire;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefabs;

    [Header("Ammo Settings")]
    [SerializeField] private int ammo = 200;
    [SerializeField] private List<BulletEntry> bulletPrefabs;
    private BulletType currentType = BulletType.Standard;

    private GameObject GetCurrentPrefab()
    {
        BulletEntry entry = bulletPrefabs.Find(b => b.type == currentType);
        if (entry.prefab == null)
        {
            CustomDebug.LogMessage($"No prefab found for bullet type: {currentType}");
        }
        return entry.prefab;
    }

    [Header("Fire Rate Settings")]
    [SerializeField] private float shootingRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Instantiation Points")]
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;

    public int Ammo => ammo;


    void Awake()
    {
        var map = InputActions.FindActionMap("Gameplay");
        fire = map.FindAction("fire");
    }

    void Update()
    {
        if (fire.WasPressedThisFrame())
        {
            Shoot();
        }
        bool middleButtonPressed = Mouse.current != null && Mouse.current.middleButton.wasPressedThisFrame;
        if(middleButtonPressed && currentType == BulletType.Standard)
        {
            SwapBullet(BulletType.Homing);
        }
        else if(middleButtonPressed && currentType == BulletType.Homing)
        {
            SwapBullet(BulletType.Standard);
        }
    }

    private void Shoot()
    {
        if (CanFire() && SettingsManager.instance.shotType == true)
        {
            Fire();
        }
        else if (CanFire() && SettingsManager.instance.shotType == false)
        {
            Fire();
        }
    }

    private bool CanFire()
    {
        return ammo > 0 && Time.time >= nextFireTime;
    }

    private void Fire()
    {
        GameObject prefab = GetCurrentPrefab();
        if (prefab == null)
        {
            CustomDebug.LogMessage("Fire() aborted — no valid prefab for current bullet type.");
            return;
        }

        ammo--;
        nextFireTime = Time.time + shootingRate;

        Instantiate(prefab, firePoint1.position, firePoint1.rotation);
        Instantiate(prefab, firePoint2.position, firePoint2.rotation);

        CustomDebug.LogMessage($"Fired! Ammo remaining: {ammo}");

        if (ammo <= 0)
        {
            CustomDebug.LogMessage("Out of ammo!");
        }
    }

    public void SwapBullet(BulletType newType)
    {
        currentType = newType;
    }

    public void Reload(int amount)
    {
        ammo += amount;
    }
}