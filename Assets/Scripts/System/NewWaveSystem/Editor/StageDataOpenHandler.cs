using UnityEditor;
using UnityEditor.Callbacks;

public class StageDataOpenHandler
{
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is StageData stageData)
        {
            WaveManagerWindow.OpenWithStage(stageData);
            return true; // true を返すと Unity のデフォルト挙動をキャンセル
        }

        return false;
    }
}
