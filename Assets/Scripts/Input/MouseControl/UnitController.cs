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

    private void Start()
    {
        unitMove.action.performed += _ => UnitSetState(Ally.State.MoveToNearestTarget);
        unitFreeMove.action.performed += _ => UnitSetState(Ally.State.MoveToClickPos);
        unitFollow.action.performed += _ => { if (unitSelector.ControlTarget != null) UnitSetState(Ally.State.Follow); };
        unitStop.action.performed += _ => UnitSetState(Ally.State.Stop);
        unitMoveToHive.action.performed += _ => UnitSetState(Ally.State.MoveToHive);
        openUnitPanel.action.performed += _ => OpenUnitPanel();
    }

    private void UnitSetState(Ally.State nextState)
    {
        foreach (var ally in unitSelector.SelectingAllies)
        {
            ally.Next(nextState);
            unitSelector.Deselect(ally.transform);
        }

        unitControlMenu.CloseMenu();
    }

    public void UnitSetState(int nextState)
    {
        UnitSetState((Ally.State)nextState);
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

        if (unitSelector.ControlTarget.TryGetComponent<UnitBase>(out var unit))
        {
            evolutionTreeView.ShowTree(unit.EvolutionTreeManager.EvolutionTree);
        }
        else
        {
            Debug.LogWarning("No EvolutionTree found on the control target.");
        }

        unitControlMenu.CloseMenu();
    }

    private void OnDestroy()
    {
        unitMove.action.Reset();
        unitFreeMove.action.Reset();
        unitFollow.action.Reset();
        unitStop.action.Reset();
        unitMoveToHive.action.Reset();
        openUnitPanel.action.Reset();
    }
}