using UnityEngine;

/// <summary>
/// ユニットコマンドの定義クラス
/// </summary>
[CreateAssetMenu(fileName = "UnitCommand", menuName = "ScriptableObjects/UnitCommand", order = 1)]
public class UnitCommand : ScriptableObject
{
    [SerializeField] string _commandNameKey;
    [SerializeField] Sprite _icon;
    [SerializeField] CommandTarget _targetType;

    public string CommandNameKey => _commandNameKey;
    public Sprite Icon => _icon;
    public CommandTarget TargetType => _targetType;

    public enum CommandTarget
    {
        Self,
        Ally,
        Enemy,
        Unit,
        Entity,
        Location,
        None,
        Specific
    }
}
