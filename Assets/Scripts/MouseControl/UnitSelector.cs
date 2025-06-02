using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder((int)ExecutionOrder.UnitSelector)]
public class UnitSelector : MonoBehaviour
{
    [SerializeField] MouseInputManager mouseInputManager;
    [SerializeField] Image mouseDragArea;
    [SerializeField] GameObject selectMarker;
    [SerializeField] GameObject targetMarker;
    readonly List<Transform> targets = new();
    readonly List<GameObject> markers = new();
    readonly Collider[] cols = new Collider[100];

    public List<Ally> SelectingAllies => targets.Select(x => x.GetComponent<Ally>()).ToList();
    public Transform Hovered { get; private set; }
    public Transform ControlTarget { get; private set; }

    public event Action OnSelectControlTarget;

    Action gizmo;

    private void Start()
    {
        mouseDragArea.enabled = false;

        mouseInputManager.OnMouseUp += () =>
        {
            mouseDragArea.enabled = false;
        };

        mouseInputManager.OnStartDrag += () =>
        {
            mouseDragArea.enabled = true;
        };

        mouseInputManager.OnClicked += () =>
        {
            if (!mouseInputManager.IsMouseHoveringUI)
            {
                ControlTarget = null;

                if (TryGetClickedEntity(out var entity))
                {
                    if (!targets.Contains(entity))
                    {
                        targets.Add(entity);
                    }
                    else
                    {
                        targets.Remove(entity);
                    }
                }
                else
                {
                    targets.Clear();
                }
            }
        };

        mouseInputManager.OpenMenu += () =>
        {
            SelectClickedForControl();
        };
    }

    void Update()
    {
        if (mouseInputManager.IsDragging)
        {
            Drag();
        }
        else
        {
            if (RaycastUnitOnMouse(out var selectHit))
            {
                selectMarker.transform.position = selectHit.transform.position;
                selectMarker.SetActive(true);
                Hovered = selectHit.transform;
            }
            else
            {
                selectMarker.SetActive(false);
            }
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

    /// <summary>
    /// クリックしたユニットを選択する。
    /// </summary>
    bool TryGetClickedEntity(out Transform entity)
    {
        if (RaycastUnitOnMouse(out var targetHit))
        {
            entity = targetHit.rigidbody.transform;
            return true;
        }
        else
        {
            entity = null;
            return false;
        }
    }

    /// <summary>
    /// クリックしたユニットを選択する。
    /// </summary>
    void SelectClickedForControl()
    {

        if (RaycastUnitOnMouse(out var targetHit))
        {
            ControlTarget = targetHit.rigidbody.transform;
            OnSelectControlTarget?.Invoke();
        }
        else
        {
            ControlTarget = null;
        }
    }

    bool RaycastUnitOnMouse(out RaycastHit hit)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return (Physics.Raycast(ray, out hit, float.MaxValue) && (hit.transform.CompareTag("Ally") || hit.transform.CompareTag("Enemy")));
    }

    // ドラッグしている間、選択範囲を表示し、範囲内のユニットを選択する。
    void Drag()
    {
        targets.Clear();

        var dragStartMousePos = mouseInputManager.DragStartMousePos;
        var dragEndMousePos = mouseInputManager.DragEndMousePos;

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
                    targets.Add(col.attachedRigidbody.transform);
                }
                else
                {
                    Debug.DrawLine(col.transform.position, col.transform.position + Vector3.up * 10, Color.red);
                }
            }
        }

        /// <summary>
        /// 視錐台内に入っているか
        /// </summary>
        static bool CheckUnitInArea(Collider collider)
        {
            if (collider.CompareTag("Ally") || collider.CompareTag("Enemy"))
            {
                var bounds = collider.bounds;
                var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
                return GeometryUtility.TestPlanesAABB(planes, bounds);
            }
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        gizmo?.Invoke();

        gizmo = null;
    }

    public void Deselect(Transform element)
    {
        targets.Remove(element);
    }
}