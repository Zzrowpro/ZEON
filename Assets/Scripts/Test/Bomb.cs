using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private CircleCollider2D range;
    private LineRenderer line; 
    [SerializeField] private int segments;
    private float radius;
    void Awake()
    {
        range = GetComponent<CircleCollider2D>();
        line = GetComponent<LineRenderer>();
        radius = range.radius; //getting radius
        line.useWorldSpace = false;
        line.loop = true;
        line.positionCount = segments;
        CreateCircle();
    }

    void CreateCircle()
    {
        Vector3 [] points = new Vector3 [segments];
        for(int i = 0; i < segments; i++)
        {
            float angle = ((float)i / (float)segments)*(math.PI * 2f);
            points[i] = new Vector3(MathF.Cos(angle) * radius, Mathf.Sin(angle), 0f);
        }
        line.SetPositions(points);
    }
}