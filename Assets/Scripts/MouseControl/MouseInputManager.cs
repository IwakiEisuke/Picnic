using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// É}ÉEÉXì¸óÕÇÃä«óù
/// </summary>
[Serializable]
public class MouseInputManager
{
    public event Action OnMouseDown;
    public event Action OnMouseUp;
    public event Action OnStartDrag;
    public event Action OnClicked;
    public event Action OnRightClicked;

    public float startDragDistance = 50;
    [HideInInspector] public Vector3 dragStartMousePos;
    [HideInInspector] public Vector3 dragEndMousePos;

    [SerializeField] InputActionReference mousePress;
    [SerializeField] InputActionReference mousePressCtrl;
    [SerializeField] InputActionReference mousePoint;
    [SerializeField] InputActionReference mouseRight;

    public bool IsDragging => _isDragging;
    public bool IsMouseHoveringUI => _isMouseHoveringUI;

    bool _isDragging;
    bool _canDrag;
    bool _isMouseHoveringUI;

    public void Init()
    {
        mousePress.action.started += (context) =>
        {
            //Debug.Log("drag start");
            dragStartMousePos = mousePoint.action.ReadValue<Vector2>();
            OnMouseDown?.Invoke();
            _canDrag = !_isMouseHoveringUI;
        };

        mousePress.action.canceled += (context) =>
        {
            //Debug.Log("drag complete");
            OnMouseUp?.Invoke();
            if (!IsDragging) OnClicked?.Invoke();
            _isDragging = false;
        };

        mouseRight.action.canceled += (context) =>
        {
            Debug.Log("right click");
            OnRightClicked?.Invoke();
        };

        mousePress.action.started += (context) => Debug.Log("Click started");
        mousePress.action.performed += (context) => Debug.Log("Click performed");
        mousePress.action.canceled += (context) => Debug.Log("Click canceled");

        mousePressCtrl.action.started += (context) => Debug.Log("Click + Ctrl started");
        mousePressCtrl.action.performed += (context) => Debug.Log("Click + Ctrl performed");
        mousePressCtrl.action.canceled += (context) => Debug.Log("Click + Ctrl canceled");
    }

    public void Update()
    {
        _isMouseHoveringUI = EventSystem.current.IsPointerOverGameObject();

        if (mousePress.action.IsPressed() && _canDrag)
        {
            dragEndMousePos = mousePoint.action.ReadValue<Vector2>();
            if (!IsDragging && Vector3.Distance(dragStartMousePos, Input.mousePosition) > startDragDistance)
            {
                _isDragging = true;
                OnStartDrag?.Invoke();
            }
        }
    }

    public void OnDestroy()
    {
        OnMouseDown = null;
        OnMouseUp = null;
        OnStartDrag = null;
        OnClicked = null;

        mousePress.action.Reset();
        mousePressCtrl.action.Reset();
        mousePoint.action.Reset();
        mouseRight.action.Reset();
    }
}