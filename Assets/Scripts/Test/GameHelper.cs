using System;
using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// Useful methods than can be reused in different projects
/// </summary>
public static class GameHelper
{
    public static Vector2 DirectionFromTo(Vector2 startPos, Vector2 endPos)
    {
        Vector2 direction = (startPos - endPos).normalized;
        return direction;
    }

    public static Vector3 DirectionFromTo(Vector3 startPos, Vector3 endPos)
    {
        Vector3 direction = (startPos - endPos).normalized;
        return direction;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2, bool clamp)
    {
        return from2 + (value - from1)/(to1-from1) * (to2 - from2);
    }

}   