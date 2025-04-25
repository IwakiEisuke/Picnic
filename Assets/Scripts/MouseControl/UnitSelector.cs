using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] MouseInputManager mouseInputManager = new();
    [SerializeField] Image mouseDragArea;
    [SerializeField] GameObject selectMarker;
    [SerializeField] GameObject targetMarker;
    [SerializeField] float startDragDistance = 50;
    readonly List<Transform> targets = new();
    readonly List<GameObject> markers = new();
    readonly Collider[] cols = new Collider[100];

    Action gizmo;

    private void Start()
    {
        mouseInputManager.OnMouseUp += () =>
        {
            if (!mouseInputManager.IsDragging)
            {
                TargetSelectAction();
            }
        };
    }

    void Update()
    {
        mouseInputManager.Update();

        if (mouseInputManager.IsDragging)
        {
            mouseDragArea.enabled = true;
            Drag();
        }
        else
        {
            mouseDragArea.enabled = false;
        }

        if (mouseInputManager.IsDragging && RaycastUnitOnMouse(out var selectHit))
        {
            selectMarker.transform.position = selectHit.transform.position;
            selectMarker.SetActive(true);
        }
        else
        {
            selectMarker.SetActive(false);
        }

        foreach (var marker in markers)
        {
            Destroy(marker);
        }

        foreach (var target in targets)
        {
            markers.Add(Instantiate(targetMarker, target.position, Quaternion.identity));
        }
    }

    void TargetSelectAction()
    {
        if (RaycastUnitOnMouse(out var targetHit))
        {
            var transform = targetHit.collider.transform;
            if (!targets.Contains(transform)) targets.Add(transform);
        }
        else
        {
            targets.Clear();
        }
    }

    void Drag()
    {
        targets.Clear();

        var dragStartMousePos = mouseInputManager.dragStartMousePos;
        var dragEndMousePos = mouseInputManager.dragEndMousePos;

        var max = new Vector2(Mathf.Max(dragStartMousePos.x, dragEndMousePos.x), Mathf.Max(dragStartMousePos.y, dragEndMousePos.y));
        var min = new Vector2(Mathf.Min(dragStartMousePos.x, dragEndMousePos.x), Mathf.Min(dragStartMousePos.y, dragEndMousePos.y));
        mouseDragArea.rectTransform.offsetMax = max;
        mouseDragArea.rectTransform.offsetMin = min;

        var maxRay = Camera.main.ScreenPointToRay(max);
        var minRay = Camera.main.ScreenPointToRay(min);

        if (Physics.Raycast(maxRay, out var maxHit) && Physics.Raycast(minRay, out var minHit))
        {
            var center = (maxHit.point + minHit.point) / 2;
            var half = (maxHit.point - minHit.point) / 2;

            Debug.DrawRay(maxHit.point, Vector3.up, Color.cyan);
            Debug.DrawRay(minHit.point, Vector3.up, Color.cyan);

            Debug.DrawRay(center, Vector3.up, Color.magenta);
            Debug.DrawLine(center, center + half);

            var cameraRot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            var i_cameraRot = Quaternion.AngleAxis(-Camera.main.transform.eulerAngles.y, Vector3.up);

            half.y = 10f;

            var rotatedHalf = i_cameraRot * half;

            gizmo += () =>
            {
                var rotatedCenter = i_cameraRot * center;
                Gizmos.matrix = Matrix4x4.Rotate(cameraRot);
                Gizmos.color = new Color(0, 1f, 1f, 0.3f);
                Gizmos.DrawCube(rotatedCenter, rotatedHalf * 2);
            };

            for (int i = 0; i < Physics.OverlapBoxNonAlloc(center, rotatedHalf, cols, cameraRot); i++)
            {
                var col = cols[i];

                if (CheckUnitInArea(col))
                {
                    Debug.DrawLine(col.transform.position, col.transform.position + Vector3.up * 10, Color.green);
                    targets.Add(col.transform);
                }
                else
                {
                    Debug.DrawLine(col.transform.position, col.transform.position + Vector3.up * 10, Color.red);
                }
            }
        }
    }

    bool CheckUnitInArea(Collider collider)
    {
        if (collider.CompareTag("Ally") || collider.CompareTag("Enemy"))
        {
            var bounds = collider.bounds;
            var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }
        return false;
    }

    bool RaycastUnitOnMouse(out RaycastHit hit)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return (Physics.Raycast(ray, out hit, float.MaxValue) && (hit.transform.CompareTag("Ally") || hit.transform.CompareTag("Enemy")));
    }

    private void OnDrawGizmos()
    {
        gizmo?.Invoke();

        gizmo = null;
    }
}

[Serializable]
public class MouseInputManager
{
    bool isDragging;
    public bool IsDragging => isDragging;
    public event Action OnMouseDown;
    public event Action OnMouseUp;

    public float startDragDistance = 50;
    [HideInInspector] public Vector3 dragStartMousePos;
    [HideInInspector] public Vector3 dragEndMousePos;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartMousePos = Input.mousePosition;
            OnMouseDown.Invoke();
        }

        if (Input.GetMouseButton(0) && !isDragging && Vector3.Distance(dragStartMousePos, Input.mousePosition) > startDragDistance)
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnMouseUp.Invoke();
        }

        if (isDragging)
        {
            dragEndMousePos = Input.mousePosition;
        }
    }
}