using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private Transform player;

    [Header("Timing")]
    [SerializeField] private float minInterval = 8f;
    [SerializeField] private float maxInterval = 15f;
    [SerializeField] private int maxAlive = 3;

    [Header("Placement")]
    [SerializeField] private float minDistanceFromPlayer = 4f;
    [SerializeField, Range(0f, 0.4f)] private float edgePadding = 0.1f;

    private readonly List<GameObject> alive = new List<GameObject>();
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        if (player == null)
        {
            var pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) player = pc.transform;
        }
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            alive.RemoveAll(p => p == null);
            if (alive.Count >= maxAlive || powerUps.Length == 0) continue;

            if (TryGetSpawnPoint(out Vector3 pos))
            {
                GameObject prefab = powerUps[Random.Range(0, powerUps.Length)];
                alive.Add(Instantiate(prefab, pos, Quaternion.identity));
            }
        }
    }

    private bool TryGetSpawnPoint(out Vector3 point)
    {
        float planeZ = player != null ? player.position.z : 0f;
        Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, planeZ));

        for (int i = 0; i < 10; i++)
        {
            Vector2 vp = new Vector2(
                Random.Range(edgePadding, 1f - edgePadding),
                Random.Range(edgePadding, 1f - edgePadding));

            Ray ray = cam.ViewportPointToRay(vp);
            if (!plane.Raycast(ray, out float dist)) continue;

            point = ray.GetPoint(dist);
            if (player == null || Vector2.Distance(point, player.position) >= minDistanceFromPlayer)
                return true;
        }

        point = Vector3.zero;
        return false;
    }
}