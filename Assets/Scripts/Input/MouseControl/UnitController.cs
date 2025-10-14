using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 選択したユニットの制御を行うクラス
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.UnitController)]
public class UnitController : MonoBehaviour
{
    [SerializeField] UnitSelector unitSelector;
    [SerializeField] UnitControlMenu unitControlMenu;
    [SerializeField] EvolutionTreeView evolutionTreeView;

    [SerializeField] InputActionReference unitMove;
    [SerializeField] InputActionReference unitFreeMove;
    [SerializeField] InputActionReference unitFollow;
    [SerializeField] InputActionReference unitStop;
    [SerializeField] InputActionReference unitMoveToHive;
    [SerializeField] InputActionReference openUnitPanel;

    List<InputActionWrapper> actions;

    private void Start()
    {
        actions = new List<InputActionWrapper>
        {
            //new(unitMove, _ => UnitSetState(Ally.State.MoveToNearestTarget)),
            //new(unitFreeMove, _ => UnitSetState(Ally.State.MoveToClickPos)),
            //new(unitFollow, _ => { if (unitSelector.ControlTarget != null) UnitSetState(Ally.State.Follow); }),
            //new(unitStop, _ => UnitSetState(Ally.State.Stop)),
            //new(unitMoveToHive, _ => UnitSetState(Ally.State.MoveToHive)),
            new(openUnitPanel, _ => OpenUnitPanel()),
        };
    }

    public void ExecuteCommand(UnitCommand command, IReadOnlyList<UnitBase> targets)
    {
        foreach (var unit in targets)
        {
            if (unit == null) continue;

            if (unit.TryGetComponent<CommandExecutor>(out var executor))
            {
                executor.Next(command);
            }
        }

        unitSelector.ClearSelecting();
    }

    public void OpenUnitPanel()
    {
        if (unitSelector.ControlTarget == null)
        {
            Debug.LogWarning("No control target selected.");
            return;
        }

        if (evolutionTreeView == null)
        {
            Debug.LogWarning("EvolutionTreeView is not assigned.", this);
            return;
        }

        if (unitSelector.ControlTarget.TryGetComponent<EvolutionTreeManager>(out var evo))
        {
            evolutionTreeView.ShowTree(evo.EvolutionTree);
        }
        else
        {
            Debug.LogWarning("No EvolutionTree found on the control target.");
        }

        unitControlMenu.CloseMenu();
    }

    private void OnDestroy()
    {
        foreach (var action in actions)
        {
            action.Unregister();
        }
    }
}