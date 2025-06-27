using System;
using UnityEngine;

public class DebugUtility : MonoBehaviour
{
    static event Action Gizmo;

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        new GameObject("[DebugUtility]").AddComponent<DebugUtility>();
    }

    public static void DrawWireBoxOriented(Vector3 center, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        Gizmo += () =>
        {
            Gizmos.matrix = Matrix4x4.Translate(center) * Matrix4x4.Rotate(orientation);
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, halfExtents * 2);
        };
    }

    public static void DrawBoxOriented(Vector3 center, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        Gizmo += () =>
        {
            Gizmos.matrix = Matrix4x4.Translate(center) * Matrix4x4.Rotate(orientation);
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, halfExtents * 2);
        };
    }

    public static void DrawSphere(Vector3 center, float radius, Color color)
    {
        Gizmo += () =>
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(center, radius);
        };
    }

    private void OnDrawGizmos()
    {
        Gizmo?.Invoke();

        Gizmo = null;
    }
}
