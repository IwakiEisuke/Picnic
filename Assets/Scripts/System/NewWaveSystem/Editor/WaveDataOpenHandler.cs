using UnityEditor;
using UnityEditor.Callbacks;

public static class WaveDataOpenHandler
{
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is WaveData wave)
        {
            WaveEditorWindow.OpenWithWaveData(wave);
            return true; // ハンドル済み
        }

        return false; // 他のアセットは無視
    }
}
