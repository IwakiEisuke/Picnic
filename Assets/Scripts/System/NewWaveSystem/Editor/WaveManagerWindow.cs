using System.Collections.Generic;
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

    public static void OpenWithStage(StageData stage)
    {
        var window = GetWindow<WaveManagerWindow>();
        window.titleContent = new GUIContent("Wave Manager");
        window.selectedStageData = stage;
        window.Show();
    }

    private void SetupReorderableList()
    {
        if (selectedStageData == null) return;

        reorderableWaveList = new ReorderableList(selectedStageData.waves, typeof(WaveData), true, true, true, true);

        reorderableWaveList.drawHeaderCallback = (Rect rect) =>
        {
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
                RemoveWave(selectedStageData, wave);
            }
        };

        reorderableWaveList.onReorderCallback = (list) =>
        {
            EditorUtility.SetDirty(selectedStageData);
        };

        reorderableWaveList.onAddCallback = (ReorderableList list) =>
        {
            // ScriptableObjectとして新規WaveData作成
            WaveData newWave = CreateNewWave(selectedStageData);
            selectedStageData.waves.Add(newWave);
            EditorUtility.SetDirty(selectedStageData);
        };

        reorderableWaveList.onRemoveCallback = (ReorderableList list) =>
        {
            if (list.index >= 0 && list.index < selectedStageData.waves.Count)
            {
                WaveData waveToRemove = selectedStageData.waves[list.index];
                RemoveWave(selectedStageData, waveToRemove);
            }
        };
    }

    WaveData CreateNewWave(StageData stage)
    {
        var wave = ScriptableObject.CreateInstance<WaveData>();
        wave.name = $"Wave_{selectedStageData.waves.Count}";
        wave.parentStage = stage;
        wave.spawnEvents = new List<EnemySpawnEvent>();
        AssetDatabase.AddObjectToAsset(wave, stage); // ステージに内包させる場合
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return wave;
    }

    void RemoveWave(StageData stageData, WaveData wave)
    {
        if (EditorUtility.DisplayDialog("Delete Wave", "このウェーブを削除しますか？", "はい", "いいえ"))
        {
            if (stageData.waves.Contains(wave))
            {
                stageData.waves.Remove(wave);

                // 必ず参照を切ってから削除する
                EditorUtility.SetDirty(stageData);  // 変更マーク
                AssetDatabase.RemoveObjectFromAsset(wave); // これが安全に外す手段
                Object.DestroyImmediate(wave, true);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
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
