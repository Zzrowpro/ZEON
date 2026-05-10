using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnEntry
{
    public GameObject prefab;
    public float weight = 1f;
}

public class BallSpawner : MonoBehaviour
{
    public List<SpawnEntry> myObstacles;
    public int amount = 20;
    public Transform player;

    public float spawnRadius = 50f;
    public float despawnRadius = 60f;
    public float spawnInterval = 2f;
    public float minSpawnDistance = 5f;

    private List<GameObject> spawnedObjects = new();
    private float timer;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float dist = Random.Range(minSpawnDistance, spawnRadius);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
            Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0);

            GameObject prefab = PickWeightedRandom();
            if (prefab != null)
                spawnedObjects.Add(Instantiate(prefab, spawnPos, Quaternion.identity));
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0;

        CheckDespawn();

        if (spawnedObjects.Count < amount)
            TrySpawn();
    }

    void TrySpawn()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float dist = Random.Range(minSpawnDistance, spawnRadius);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0);

        GameObject prefab = PickWeightedRandom();
        if (prefab != null)
            spawnedObjects.Add(Instantiate(prefab, spawnPos, Quaternion.identity));
    }

    void CheckDespawn()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null ||
                Vector2.Distance(player.position, spawnedObjects[i].transform.position) > despawnRadius)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    GameObject PickWeightedRandom()
    {
        float totalWeight = 0f;
        foreach (var entry in myObstacles)
            totalWeight += entry.weight;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in myObstacles)
        {
            cumulative += entry.weight;
            if (roll < cumulative)
                return entry.prefab;
        }

        return null;
    }
}