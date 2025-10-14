using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドメニューの生成・破棄を管理するクラス
/// </summary>
public class CommandMenuGenerator : MonoBehaviour
{
    [SerializeField] UnitsCommandCollector _commandCollector;
    [SerializeField] CommandButtonFactory _commandButtonFactory;
    [SerializeField] UnitController _unitController;
    [SerializeField] UnitControlMenu _unitControlMenu;

    readonly List<GameObject> _createdCommandButtons = new();

    public void CreateMenu()
    {
        _commandCollector.CollectCommands();
        foreach (var command in _commandCollector.CollectedCommands)
        {
            var b = _commandButtonFactory.CreateCommandCell(command, () =>
            {
                // ここにコマンド実行処理を追加
                _unitControlMenu.CloseMenu();
            });

            _createdCommandButtons.Add(b);
        }
    }

    public void ClearMenu()
    {
        foreach (var button in _createdCommandButtons)
        {
            Destroy(button);
        }
        _createdCommandButtons.Clear();
    }
}
