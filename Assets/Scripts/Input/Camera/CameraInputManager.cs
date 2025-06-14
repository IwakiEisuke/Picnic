using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// カメラ入力の管理
/// </summary>
[CreateAssetMenu(menuName = "InputManager/CameraInput")]
public class CameraInputManager : InputManagerBase
{
    [SerializeField] bool debugMode = false;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float zoomSpeed = 5f;

    [SerializeField] InputActionReference cameraMoveAction;
    [SerializeField] InputActionReference cameraZoomAction;

    public event Action<Vector2> OnCameraMove;
    public event Action<float> OnCameraZoom;

    public override void Init()
    {
        void Move(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnCameraMove?.Invoke(moveSpeed * moveInput);
            if (debugMode) Debug.Log($"Camera Move Action Performed: {moveInput}");
        }

        void Zoom(InputAction.CallbackContext context)
        {
            float zoomInput = context.ReadValue<Vector2>().y;
            OnCameraZoom?.Invoke(zoomSpeed * zoomInput);
            if (debugMode) Debug.Log($"Camera Zoom Action Performed: {zoomInput}");
        }

        cameraMoveAction.action.performed += Move;
        cameraMoveAction.action.canceled += Move;

        cameraZoomAction.action.performed += Zoom;
        cameraZoomAction.action.canceled += Zoom;
    }

    public override void ResetActions()
    {
        OnCameraMove = null;
        OnCameraZoom = null;
    }

    public override void Update()
    {
        
    }
}
