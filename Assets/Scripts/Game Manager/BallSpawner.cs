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
    [SerializeField] private List<SpawnEntry> myObstacles;
    [SerializeField] private int amount = 20;
    [SerializeField] private Transform player;

    [SerializeField] private float spawnRadius = 50f;
    [SerializeField] private float despawnRadius = 60f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpawnDistance = 5f;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private bool fadeInitialSpawns = false;

    private List<GameObject> spawnedObjects = new();
    private float timer;

    void Start()
    {
        for (int i = 0; i < amount; i++)
            SpawnOne(fadeInitialSpawns);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0;

        CheckDespawn();

        if (spawnedObjects.Count < amount)
            SpawnOne(true);
    }

    void SpawnOne(bool fade)
    {
        GameObject prefab = PickWeightedRandom();
        if (prefab == null) return;

        float angle = Random.Range(0f, Mathf.PI * 2);
        float dist = Mathf.Sqrt(Random.Range(minSpawnDistance * minSpawnDistance,
                                             spawnRadius * spawnRadius));
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (fade)
        {
            if (!obj.TryGetComponent(out FadeIn fadeIn))
                fadeIn = obj.AddComponent<FadeIn>();
            fadeIn.duration = fadeDuration;
        }

        spawnedObjects.Add(obj);
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