using System;
using UnityEngine;

public class DebugUtility : MonoBehaviour
{
    static event Action Gizmos;

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        new GameObject("[DebugUtility]").AddComponent<DebugUtility>();
    }

    public static void DrawWireBoxOriented(Vector3 center, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        Gizmos += () =>
        {
            UnityEngine.Gizmos.matrix = Matrix4x4.Translate(center) * Matrix4x4.Rotate(orientation);
            color.a = 0.5f;
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawCube(Vector3.zero, halfExtents * 2);
        };
    }

    public static void DrawBoxOriented(Vector3 center, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        Gizmos += () =>
        {
            UnityEngine.Gizmos.matrix = Matrix4x4.Translate(center) * Matrix4x4.Rotate(orientation);
            color.a = 0.5f;
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawCube(Vector3.zero, halfExtents * 2);
        };
    }

    private void OnDrawGizmos()
    {
        Gizmos?.Invoke();

        Gizmos = null;
    }
}
