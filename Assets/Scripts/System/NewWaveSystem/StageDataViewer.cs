using UnityEditor;
using UnityEngine;

/// <summary>
/// スポーンポイントをシーンビューに表示するコンポーネント
/// </summary>
[ExecuteInEditMode]
public class StageDataViewer : MonoBehaviour
{
    public StageData stageData;

    private void OnDrawGizmos()
    {
        if (stageData == null || stageData.spawnPoints == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < stageData.spawnPoints.Count; i++)
        {
            var point = stageData.spawnPoints[i];
            Vector3 worldPos = point.position;
            Gizmos.DrawSphere(worldPos, 0.3f);
#if UNITY_EDITOR
            // 座標ラベルを表示
            Handles.Label(worldPos + Vector3.up * 0.5f, $"#{i}");
#endif
        }
    }
}
