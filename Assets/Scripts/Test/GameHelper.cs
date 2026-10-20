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

    /// <summary>
    /// Scales a value from one interval to another
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from1"></param>
    /// <param name="to1"></param>
    /// <param name="from2"></param>
    /// <param name="to2"></param>
    /// <param name="clamp"></param>
    /// <returns>returs a float </returns>
    public static float Remap(float value, float from1, float to1, float from2, float to2, bool clamp)
    {
        return from2 + (value - from1)/(to1-from1) * (to2 - from2);
    }

    /// <summary>
    /// Smoothly rotates the rigibody to target
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="rb2D"></param>
    /// <param name="rotationSpeed"></param>
    public static void SmoothRotationToPoint(Transform target, Rigidbody2D rb2D, float rotationSpeed)
    {
        Vector2 position = target.position;
        float angle = MathF.Atan2(position.y, position.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.LerpAngle(rb2D.rotation, angle,rotationSpeed * Time.fixedDeltaTime);
        rb2D.MoveRotation(smoothedAngle);
    }

    static public void SnapToGrid(Vector2 position, float gridSize)
    {
        
    }

}   