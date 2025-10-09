using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// マウス入力の管理
/// </summary>
[CreateAssetMenu(menuName = "InputManager/MouseInput")]
public class MouseInputManager : InputManagerBase
{
    public bool IsDragging => _isDragging;
    public bool IsMouseHoveringUI => _isMouseHoveringUI;
    public Vector3 DragStartMousePos => _dragStartMousePos;
    public Vector3 DragEndMousePos => _dragEndMousePos;

    public event Action OnMouseDown;
    public event Action OnMouseUp;
    public event Action OnStartDrag;
    public event Action OnClicked;
    public event Action OpenMenu;
    public event Action CloseMenu;
    public event Action OnMouseClickedWithoutUI;

    public float startDragDistance = 50;

    [SerializeField] InputActionReference mousePress;
    [SerializeField] InputActionReference mousePressCtrl;
    [SerializeField] InputActionReference mousePoint;
    [SerializeField] InputActionReference openMenu;

    [SerializeField] bool enableDebugInput;

    Vector3 _dragStartMousePos;
    Vector3 _dragEndMousePos;

    bool _isDragging;
    bool _canDrag;
    bool _isMouseHoveringUI;

    public override void Init()
    {
        mousePress.action.started += (context) =>
        {
            _dragStartMousePos = mousePoint.action.ReadValue<Vector2>();
            OnMouseDown?.Invoke();
            _canDrag = !_isMouseHoveringUI;
        };

        mousePress.action.canceled += (context) =>
        {
            OnMouseUp?.Invoke();
            if (!IsDragging) OnClicked?.Invoke();
            _isDragging = false;

            if (_canDrag)
            {
                OnMouseClickedWithoutUI?.Invoke();
                CloseMenu?.Invoke();
            }
        };

        openMenu.action.performed += (context) =>
        {
            OpenMenu?.Invoke();
        };

#if UNITY_EDITOR
        if (enableDebugInput)
        {
            mousePress.action.started += (context) => Debug.Log("Click started");
            mousePress.action.performed += (context) => Debug.Log("Click performed");
            mousePress.action.canceled += (context) => Debug.Log("Click canceled");

            mousePressCtrl.action.started += (context) => Debug.Log("Click + Ctrl started");
            mousePressCtrl.action.performed += (context) => Debug.Log("Click + Ctrl performed");
            mousePressCtrl.action.canceled += (context) => Debug.Log("Click + Ctrl canceled");
        }
#endif
    }

    public override void Update()
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("EventSystemコンポーネントが存在しません。マウス操作は正常に動作しません");
            return;
        }
        _isMouseHoveringUI = EventSystem.current.IsPointerOverGameObject();

        if (mousePress.action.IsPressed() && _canDrag)
        {
            _dragEndMousePos = mousePoint.action.ReadValue<Vector2>();
            if (!IsDragging && Vector3.Distance(_dragStartMousePos, Input.mousePosition) > startDragDistance)
            {
                _isDragging = true;
                OnStartDrag?.Invoke();
                CloseMenu?.Invoke();
            }
        }
    }

    public override void ResetActions()
    {
        OnMouseDown = null;
        OnMouseUp = null;
        OnStartDrag = null;
        OnClicked = null;
        OpenMenu = null;
        CloseMenu = null;
        OnMouseClickedWithoutUI = null;
    }
}