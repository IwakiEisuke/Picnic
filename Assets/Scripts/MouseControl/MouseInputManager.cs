using System;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// ƒ}ƒEƒX“ü—Í‚ÌŠÇ—
/// </summary>
[Serializable]
public class MouseInputManager
{
    bool isDragging;
    public bool IsDragging => isDragging;
    public event Action OnMouseDown;
    public event Action OnMouseUp;
    public event Action OnStartDrag;
    public event Action OnClicked;

    public float startDragDistance = 50;
    [HideInInspector] public Vector3 dragStartMousePos;
    [HideInInspector] public Vector3 dragEndMousePos;

    [SerializeField] InputActionReference mousePress;
    [SerializeField] InputActionReference mousePressCtrl;

    public void Init()
    {
        mousePress.action.started += (context) =>
        {
            //Debug.Log("drag start");
            dragStartMousePos = Mouse.current.position.value;
            OnMouseDown?.Invoke();
        };

        mousePress.action.canceled += (context) =>
        {
            //Debug.Log("drag complete");
            OnMouseUp?.Invoke();
            if (!isDragging) OnClicked?.Invoke();
            isDragging = false;
        };

        mousePress.action.performed += (context) =>
        {
            //Debug.Log("dragging");
            dragEndMousePos = Mouse.current.position.value;
            if (!isDragging && Vector3.Distance(dragStartMousePos, Input.mousePosition) > startDragDistance)
            {
                isDragging = true;
                OnStartDrag?.Invoke();
            }
        };

        //mousePress.action.started += (context) => Debug.Log("Click started");
        //mousePress.action.performed += (context) => Debug.Log("Click performed");
        //mousePress.action.canceled += (context) => Debug.Log("Click canceled");

        mousePressCtrl.action.started += (context) => Debug.Log("Click + Ctrl started");
        mousePressCtrl.action.performed += (context) => Debug.Log("Click + Ctrl performed");
        mousePressCtrl.action.canceled += (context) => Debug.Log("Click + Ctrl canceled");
    }

    public void OnDestroy()
    {
        OnMouseDown = null;
        OnMouseUp = null;
        OnStartDrag = null;
        OnClicked = null;
    }
}