using UnityEditor;
using UnityEngine;

/// <summary>
/// シーンビューからスポーンポイントを操作できるようにするエディター
/// </summary>
[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    private StageData data;

    // StageDataアセットが選択されたときに呼び出されるメソッド
    // CanEditMultipleObjects属性がある場合のみ複数選択した際に呼び出される
    private void OnEnable()
    {
        data = (StageData)target;
        SceneView.duringSceneGui += DrawHandle;
    }

    // StageDataアセットの選択が解除されたときに呼び出されるメソッド
    private void OnDisable()
    {
        SceneView.duringSceneGui -= DrawHandle;
    }

    private void DrawHandle(SceneView sceneView)
    {
        if (data == null || data.spawnPoints == null) return;

        Handles.color = Color.green;

        for (int i = 0; i < data.spawnPoints.Count; i++)
        {
            // Inspectorや、ハンドルによる”操作”を検出するためのチェックの開始
            EditorGUI.BeginChangeCheck();

            // ワールド上のハンドルを表示して移動を検出
            // PositionHandle... ハンドルを表示して、操作後のハンドル位置を返すメソッド
            Vector3 newPos = Handles.PositionHandle(data.spawnPoints[i].position, Quaternion.identity);

            // チェック終了。チェック開始後のメソッドで描画されたGUIやHandleにて操作が行われた場合True
            if (EditorGUI.EndChangeCheck())
            {
                // アセットに変更を書き込む
                Undo.RecordObject(data, $"Move Spawn Point {i}");
                data.spawnPoints[i].position = newPos;
                EditorUtility.SetDirty(data);
            }

            // スポーンポイントの番号をラベル表示
            Handles.Label(newPos + Vector3.up * 0.5f, $"#{i}");
        }
    }
}
