using UnityEngine;

/// <summary>
/// ユニットコマンドの定義クラス
/// </summary>
[CreateAssetMenu(fileName = "UnitCommand", menuName = "ScriptableObjects/UnitCommand", order = 1)]
public class UnitCommand : ScriptableObject
{
    [SerializeField] string _commandNameKey;
    [SerializeField] Sprite _icon;
}
