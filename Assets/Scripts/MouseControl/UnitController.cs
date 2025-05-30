using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 選択したユニットの制御を行うクラス
/// </summary>
public class UnitController : MonoBehaviour
{
    UnitSelector unitSelector;

    [SerializeField] InputActionReference unitMove;
    [SerializeField] InputActionReference unitFreeMove;

    private void Start()
    {
        unitSelector = FindAnyObjectByType<UnitSelector>();

        unitMove.action.performed += UnitMove;
        unitFreeMove.action.performed += UnitFreeMove;
    }

    private void UnitFreeMove(InputAction.CallbackContext context)
    {
        unitSelector.SelectingAllies.ForEach(ally =>
        {
            ally.MoveToNearestTarget();
        });
    }

    private void UnitMove(InputAction.CallbackContext context)
    {
        unitSelector.SelectingAllies.ForEach(ally =>
        {
            ally.MoveToClickPos();
        });
    }

    

    private void OnDestroy()
    {
        unitMove.action.performed -= UnitMove;
    }
}
