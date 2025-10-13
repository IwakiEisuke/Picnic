using Entity.CommandActions;
using UnityEngine;

/// <summary>
/// コマンドに対応するコマンドアクションを実行するクラス
/// </summary>
public class CommandExecutor : MonoBehaviour
{
    [SerializeField] CommandTranslator _translator;
    [SerializeField] UnitCommand _defaultCommand;

    CommandActionBase currentCommandAction;

    private void Start()
    {
        SetDefault();
    }

    private void Update()
    {
        if (currentCommandAction != null)
        {
            currentCommandAction.UpdateAction();
        }
    }

    public void Next(UnitCommand command)
    {
        var data = _translator.Translate(command);
        if (data == null)
        {
            Debug.LogWarning($"CommandTranslator: No CommandAction found for {command}");
            return;
        }

        if (currentCommandAction != null)
        {
            currentCommandAction.Exit();
        }

        currentCommandAction = data;
        currentCommandAction.Enter();
    }

    public void SetDefault()
    {
        Next(_defaultCommand);
    }
}
