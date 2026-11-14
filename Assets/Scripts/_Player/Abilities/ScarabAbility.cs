using UnityEngine;

public class ScarabAbility : Ability
{
    [Header("Scarab Settings")]
    [SerializeField] private GameObject scarabPrefab;
    [SerializeField] private int scarabCount = 2;
    [SerializeField] private bool canSpawnScarab = true;

    private GameObject _player;
    private Transform _scarabSpawnPoint;

    void Awake()
    {
        _player = GameObject.Find("Player");
        _scarabSpawnPoint = _player.transform;
    }

    protected override void Activate()
    {
        for (int i = 0; i < scarabCount; i++)
        {    
            SpawnScarab();
        }
    }

    private void SpawnScarab()
    {
       if (scarabPrefab == null)    
       {
           CustomDebug.LogMessage("Abilities: No scarab prefab assigned!");
           return;
       }
       Instantiate(scarabPrefab, _scarabSpawnPoint.position, _scarabSpawnPoint.rotation);
       CustomDebug.LogMessage("Scarab spawned!");
    }
}