using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class WaveManagerWindow : EditorWindow
{
    private ReorderableList reorderableWaveList;
    private StageData selectedStageData;

    [MenuItem("Tools/Wave Manager")]
    public static void ShowWindow()
    {
        GetWindow<WaveManagerWindow>("Wave Manager");
    }

    private void SetupReorderableList()
    {
        if (selectedStageData == null) return;

        reorderableWaveList = new ReorderableList(selectedStageData.waves, typeof(WaveData), true, true, true, true);

        reorderableWaveList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Waves");
        };

        reorderableWaveList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var wave = selectedStageData.waves[index];
            selectedStageData.waves[index] = (WaveData)EditorGUI.ObjectField(
                new Rect(rect.x, rect.y + 2, rect.width - 60, EditorGUIUtility.singleLineHeight),
                wave, typeof(WaveData), false
            );

            if (GUI.Button(new Rect(rect.x + rect.width - 55, rect.y + 2, 25, EditorGUIUtility.singleLineHeight), "Edit"))
            {
                WaveEditorWindow.OpenWithWaveData(wave);
                var window = EditorWindow.GetWindow<WaveEditorWindow>();
                window.Focus();
            }

            if (GUI.Button(new Rect(rect.x + rect.width - 30, rect.y + 2, 25, EditorGUIUtility.singleLineHeight), "X"))
            {
                if (EditorUtility.DisplayDialog("Delete Wave", "このウェーブを削除しますか？", "はい", "いいえ"))
                {
                    var assetPath = AssetDatabase.GetAssetPath(wave);
                    selectedStageData.waves.RemoveAt(index);
                    AssetDatabase.DeleteAsset(assetPath);
                    EditorUtility.SetDirty(selectedStageData);
                    AssetDatabase.SaveAssets();
                }
            }
        };

        reorderableWaveList.onReorderCallback = (list) =>
        {
            EditorUtility.SetDirty(selectedStageData);
        };

        reorderableWaveList.onAddCallback = (ReorderableList list) =>
        {
            // ScriptableObjectとして新規WaveData作成
            WaveData newWave = ScriptableObject.CreateInstance<WaveData>();

            // アセットとして保存
            string stageAssetPath = AssetDatabase.GetAssetPath(selectedStageData);
            string stageFolder = System.IO.Path.GetDirectoryName(stageAssetPath);
            string waveAssetPath = AssetDatabase.GenerateUniqueAssetPath($"{stageFolder}/Wave_{selectedStageData.waves.Count}.asset");

            AssetDatabase.CreateAsset(newWave, waveAssetPath);
            AssetDatabase.SaveAssets();

            selectedStageData.waves.Add(newWave);
            EditorUtility.SetDirty(selectedStageData);
        };

    }


    private Vector2 scroll;

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Wave Manager", EditorStyles.boldLabel);

        selectedStageData = (StageData)EditorGUILayout.ObjectField("Stage Data", selectedStageData, typeof(StageData), false);

        if (selectedStageData == null)
        {
            EditorGUILayout.HelpBox("ステージデータを選択してください。", MessageType.Info);
            return;
        }

        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        if (selectedStageData != null)
        {
            if (reorderableWaveList == null)
                SetupReorderableList();

            reorderableWaveList.DoLayoutList();
        }

        EditorGUILayout.EndScrollView();
    }

    private void Swap(int indexA, int indexB)
    {
        var tmp = selectedStageData.waves[indexA];
        selectedStageData.waves[indexA] = selectedStageData.waves[indexB];
        selectedStageData.waves[indexB] = tmp;
        EditorUtility.SetDirty(selectedStageData);
    }
}
