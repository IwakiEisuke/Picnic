using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// コマンドメニューの各コマンド実行ボタンを生成するファクトリ
/// </summary>
public class CommandButtonFactory : MonoBehaviour
{
    [SerializeField] CommandButtonController _commandButtonPrefab;
    [SerializeField] Transform _menuParent;


    public GameObject CreateCommandCell(UnitCommand command, UnityAction buttonAction)
    {
        var controller = Instantiate(_commandButtonPrefab, _menuParent);
        controller.SetIcon(command.Icon);
        controller.AddButtonAction(buttonAction);

        var commandButtonObject = controller.gameObject;
        return commandButtonObject;
    }
}
