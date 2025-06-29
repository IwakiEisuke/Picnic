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
            return true; // �n���h���ς�
        }

        return false; // ���̃A�Z�b�g�͖���
    }
}
