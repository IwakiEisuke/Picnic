using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ユニットが実行可能なコマンドを収集するクラス
/// </summary>
public class UnitsCommandCollector : MonoBehaviour
{
    [SerializeField] UnitSelector _unitSelector;

    readonly Dictionary<UnitCommand, List<UnitBase>> _commandToUnitsMap = new();
    readonly HashSet<UnitCommand> _collectedCommands = new();

    /// <summary>
    /// ユニットのいずれかが実行可能なコマンドのセット
    /// </summary>
    public IReadOnlyCollection<UnitCommand> CollectedCommands => _collectedCommands;

    /// <summary>
    /// 選択中のユニットが実行可能な全コマンドを収集し、コマンドごとに実行可能なユニット一覧を保持する
    /// </summary>
    public void CollectCommands()
    {
        // 前のデータをクリア
        Clear();

        foreach (var ally in _unitSelector.SelectingAllies)
        {
            if (ally.TryGetComponent<CommandTranslator>(out var translator))
            {
                foreach (var command in translator.GetCommands())
                {
                    AddUnitToCollections(command, ally);
                }
            }
        }
    }

    void AddUnitToCollections(UnitCommand command, UnitBase unit)
    {
        if (_commandToUnitsMap.TryGetValue(command, out var list))
        {
            list.Add(unit);
        }
        else
        {
            _commandToUnitsMap.Add(command, new List<UnitBase> { unit });
            _collectedCommands.Add(command);
        }
    }

    /// <summary>
    /// 指定のコマンドを実行可能なユニットを取得
    /// </summary>
    public IReadOnlyList<UnitBase> GetUnitsAbleToExecuteCommand(UnitCommand targetCommand)
    {
        if (_commandToUnitsMap.TryGetValue(targetCommand, out var units))
        {
            return units;
        }
        return new List<UnitBase>();
    }

    /// <summary>
    /// 追加された全データをクリアする
    /// </summary>
    void Clear()
    {
        _commandToUnitsMap.Clear();
        _collectedCommands.Clear();
    }
}