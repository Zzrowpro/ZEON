using UnityEngine;

public class PathFollower : MonoBehaviour {

    public PathCreator pathCreator;
    public float speed = 2;
    public float spacing = .1f;
    public bool faceDirection = false;

    Vector2[] points;
    float dstTravelled;

    void Start()
    {
        points = pathCreator.path.CalculateEvenlySpacedPoints(spacing);
    }

    void Update()
    {
        dstTravelled += speed * Time.deltaTime;

        float f = dstTravelled / spacing;
        int i = Mathf.FloorToInt(f);
        float t = f - i;

        int count = points.Length;
        int a, b;

        if (pathCreator.path.IsClosed)
        {
            a = i % count;
            b = (i + 1) % count;
        }
        else
        {
            a = Mathf.Min(i, count - 1);
            b = Mathf.Min(i + 1, count - 1);
        }

        Vector2 pos = Vector2.Lerp(points[a], points[b], t);
        transform.position = pos;

        if (faceDirection && a != b)
        {
            Vector2 dir = points[b] - points[a];
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }
    }
}