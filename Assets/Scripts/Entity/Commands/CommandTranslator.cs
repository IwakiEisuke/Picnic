using Entity.CommandActions;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドとコマンドアクションの橋渡しをするクラス
/// </summary>
public class CommandTranslator : MonoBehaviour
{
    [SerializeField] TranslateData[] translateDatas;

    [Serializable]
    class TranslateData
    {
        public UnitCommand command;
        public CommandActionBase commandAction;
    }

    Dictionary<UnitCommand, CommandActionBase> translateTable = new();

    private void Awake()
    {
        foreach (var data in translateDatas)
        {
            if (data.command == null)
            {
                Debug.LogWarning("CommandTranslator: Command cannot be Null");
                continue;
            }

            if (data.commandAction == null)
            {
                Debug.LogWarning($"CommandTranslator: CommandAction for {data.command} is not assigned");
                continue;
            }

            if (translateTable.ContainsKey(data.command))
            {
                Debug.LogWarning($"CommandTranslator: Duplicate command {data.command.name} found, skipping");
                continue;
            }

            data.commandAction = Instantiate(data.commandAction);
            data.commandAction.Init(gameObject);
            translateTable.Add(data.command, data.commandAction);
        }
    }

    public IEnumerable<UnitCommand> GetCommands()
    {
        foreach (var command in translateTable.Keys)
        {
            yield return command;
        }
    }

    /// <summary>
    /// コマンドからコマンドアクションに変換します。変換先が見つからない場合 null を返します
    /// </summary>
    public CommandActionBase Translate(UnitCommand command)
    {
        if (translateTable.TryGetValue(command, out var action))
        {
            return action;
        }
        return null;
    }
}
