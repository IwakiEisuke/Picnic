using System;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// É}ÉEÉXì¸óÕÇÃä«óù
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

    [SerializeField] InputActionReference mouseDrag;
    [SerializeField] InputActionReference mousePressCtrl;

    public void Init()
    {
        mouseDrag.action.started += (context) =>
        {
            //Debug.Log("drag start");
            dragStartMousePos = context.ReadValue<Vector2>();
            OnMouseDown?.Invoke();
        };

        mouseDrag.action.canceled += (context) =>
        {
            //Debug.Log("drag complete");
            OnMouseUp?.Invoke();
            if (!isDragging) OnClicked?.Invoke();
            isDragging = false;
        };

        mouseDrag.action.performed += (context) =>
        {
            //Debug.Log("dragging");
            dragEndMousePos = context.ReadValue<Vector2>();
            if (!isDragging && Vector3.Distance(dragStartMousePos, Input.mousePosition) > startDragDistance)
            {
                isDragging = true;
                OnStartDrag?.Invoke();
            }
        };

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