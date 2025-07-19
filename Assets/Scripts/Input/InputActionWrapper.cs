using System;
using UnityEngine.InputSystem;

/// <summary>
/// コールバックの解除を簡単にするためのラッパークラス
/// </summary>
public class InputActionWrapper
{
    readonly InputAction action;
    readonly Action<InputAction.CallbackContext> callback;

    public InputActionWrapper(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        this.action = action;
        this.callback = callback;
        action.performed += callback;
    }

    public void Unregister()
    {
        action.performed -= callback;
    }
}
