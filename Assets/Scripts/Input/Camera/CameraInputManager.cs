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
        cameraMoveAction.action.performed += context =>
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnCameraMove?.Invoke(moveSpeed * Time.deltaTime * moveInput);
            if (debugMode) Debug.Log($"Camera Move Action Performed: {moveInput}");
        };

        cameraZoomAction.action.performed += context =>
        {
            float zoomInput = context.ReadValue<Vector2>().y;
            OnCameraZoom?.Invoke(zoomSpeed * Time.deltaTime * zoomInput);
            if (debugMode) Debug.Log($"Camera Zoom Action Performed: {zoomInput}");
        };
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
