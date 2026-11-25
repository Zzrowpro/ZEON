using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTarget : MonoBehaviour
{
    public static readonly List<WaypointTarget> All = new();
    public static event Action<WaypointTarget> Added;
    public static event Action<WaypointTarget> Removed;

    public Sprite icon;
    public Color color = Color.white;
    public Vector3 offset = Vector3.up;   // marker floats above the object

    void OnEnable()  { All.Add(this);    Added?.Invoke(this); }
    void OnDisable() { All.Remove(this); Removed?.Invoke(this); }
}