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

    /// <summary>
    /// Registers input-action callbacks to handle mouse press lifecycle, update drag-related state, and raise the manager's public events.
    /// </summary>
    /// <remarks>
    /// - On mouse press started: records the pointer position, invokes <c>OnMouseDown</c>, and sets whether dragging is allowed based on UI hover state.
    /// - On mouse press canceled: invokes <c>OnMouseUp</c>, invokes <c>OnClicked</c> if a drag was not in progress, resets dragging state, and if dragging was allowed invokes <c>OnMouseClickedWithoutUI</c> and <c>CloseMenu</c>.
    /// - On open-menu action performed: invokes <c>OpenMenu</c>.
    /// - In the Unity Editor, when <c>enableDebugInput</c> is true, additional debug log callbacks are attached for click actions.
    /// </remarks>
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

    /// <summary>
    /// Updates pointer UI-hover state and detects the start of a drag operation while the primary mouse button is pressed.
    /// </summary>
    /// <remarks>
    /// If no EventSystem is present, logs a warning and returns without updating state. While the primary mouse button is pressed and dragging is allowed, this method updates the drag end position and, when the pointer has moved farther than <c>startDragDistance</c> from the recorded drag start position, marks dragging as started and invokes <c>OnStartDrag</c> and <c>CloseMenu</c>.
    /// </remarks>
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