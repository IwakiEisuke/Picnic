using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 選択したユニットの制御を行うクラス
/// </summary>
[DefaultExecutionOrder((int)ExecutionOrder.UnitController)]
public class UnitController : MonoBehaviour
{
    UnitSelector unitSelector;

    [SerializeField] InputActionReference unitMove;
    [SerializeField] InputActionReference unitFreeMove;
    [SerializeField] InputActionReference unitFollow;

    private void Start()
    {
        unitSelector = FindAnyObjectByType<UnitSelector>();

        unitMove.action.performed += _ => UnitSetState(Ally.State.MoveToNearestTarget);
        unitFreeMove.action.performed += _ => UnitSetState(Ally.State.MoveToClickPos);
        unitFollow.action.performed += _ => UnitSetState(Ally.State.Follow);
    }

    private void UnitSetState(Ally.State nextState)
    {
        foreach (var ally in unitSelector.SelectingAllies)
        {
            ally.Next(nextState);
            unitSelector.Deselect(ally.transform);
        }
    }

    public void UnitSetState(int nextState)
    {
        UnitSetState((Ally.State)nextState);
    }

    private void OnDestroy()
    {
        unitMove.action.Reset();
        unitFreeMove.action.Reset();
        unitFollow.action.Reset();
    }
}