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
            return true; // true ��Ԃ��� Unity �̃f�t�H���g�������L�����Z��
        }

        return false;
    }
}
