// Todo: 選択中と行動対象の制御を明確に

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
    [SerializeField] UnitControlMenu unitControlMenu;
    [SerializeField] LayerMask selectableLayers;
    [SerializeField] LayerMask controlTargetLayers;
    readonly List<Transform> selecting = new();
    readonly Collider[] cols = new Collider[100];

    public List<Ally> SelectingAllies => selecting.Where(x => x != null).Select(x => x.GetComponent<Ally>()).ToList();
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
                SetControlTarget(null);

                if (TryGetClickedEntity(out var entity))
                {
                    if (!selecting.Contains(entity))
                    {
                        Select(entity);
                    }
                    else
                    {
                        Deselect(entity);
                    }
                }
                else
                {
                    ClearSelecting();
                }
            }
        };

        mouseInputManager.OpenMenu += () =>
        {
            SelectClickedForControl();
        };
    }

    void Select(Transform target)
    {
        selecting.Add(target);
        SetEffectSelecting(target, true);
    }

    public void Deselect(Transform target)
    {
        selecting.Remove(target);
        SetEffectSelecting(target, false);
    }

    void ClearSelecting()
    {
        foreach (var s in selecting)
        {
            SetEffectSelecting(s, false);
        }
        selecting.Clear();
    }

    void SetEffectSelecting(Transform target, bool enable)
    {
        if (target == null) return;
        target.GetComponentInChildren<Renderer>().material.SetFloat("_Alpha", enable ? 1f : 0f);
    }

    void SetEffectControlTarget(Transform target, bool enable)
    {
        if (target == null) return;
        var isSelect = enable || (!enable && selecting.Contains(target));

        var material = target.GetComponentInChildren<Renderer>().material;
        material.SetFloat("_Alpha", isSelect ? 1f : 0f);
        material.SetFloat("_IsHover", enable ? 1f : 0f);
    }

    void SetControlTarget(Transform target)
    {
        if (unitControlMenu.IsMenuOpened) return;

        if (ControlTarget != null)
        {
            SetEffectControlTarget(ControlTarget, false);
        }

        ControlTarget = target;
        if (target != null) SetEffectControlTarget(ControlTarget, true);
    }

    void Update()
    {
        if (mouseInputManager.IsDragging)
        {
            Drag();
        }
        else
        {
            if (RaycastUnitOnMouse(out var selectHit, controlTargetLayers))
            {
                // ホバー表示
                selectMarker.transform.position = selectHit.transform.position;
                selectMarker.SetActive(true);

                // ホバー中のユニットを選択中のターゲットに設定
                SetControlTarget(selectHit.transform);
            }
            else
            {
                selectMarker.SetActive(false);
                SetControlTarget(selectHit.transform);
            }
        }
    }

    /// <summary>
    /// クリックしたユニットを選択する。
    /// </summary>
    bool TryGetClickedEntity(out Transform entity)
    {
        if (RaycastUnitOnMouse(out var targetHit, selectableLayers))
        {
            if (targetHit.rigidbody != null)
            {
                entity = targetHit.rigidbody.transform;
                return true;
            }
            entity = targetHit.transform;
            return true;
        }
        else
        {
            entity = null;
            return false;
        }
    }

    /// <summary>
    /// クリックしたオブジェクトを行動対象にする。
    /// </summary>
    void SelectClickedForControl()
    {

        if (RaycastUnitOnMouse(out var targetHit, controlTargetLayers))
        {
            SetEffectControlTarget(ControlTarget, false);
            if (targetHit.rigidbody != null)
            {
                ControlTarget = targetHit.rigidbody.transform;
            }
            else
            {
                ControlTarget = targetHit.transform;
            }
            SetEffectControlTarget(ControlTarget, true);
            OnSelectControlTarget?.Invoke();
        }
        else
        {
            SetEffectControlTarget(ControlTarget, false);
            ControlTarget = null;
        }
    }

    bool RaycastUnitOnMouse(out RaycastHit hit, LayerMask layer)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return (Physics.Raycast(ray, out hit, float.MaxValue, layer.value));
    }

    // ドラッグしている間、選択範囲を表示し、範囲内のユニットを選択する。
    void Drag()
    {
        ClearSelecting();

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

            for (int i = 0; i < Physics.OverlapBoxNonAlloc(center, rotatedHalf, cols, cameraRot, selectableLayers.value); i++)
            {
                var col = cols[i];

                if (CheckUnitInArea(col))
                {
                    Debug.DrawLine(col.transform.position, col.transform.position + Vector3.up * 10, Color.green);
                    if (col.attachedRigidbody != null)
                    {
                        Select(col.attachedRigidbody.transform);
                    }
                    else
                    {
                        Select(col.transform);
                    }
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
            var bounds = collider.bounds;
            var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }
    }

    private void OnDrawGizmos()
    {
        gizmo?.Invoke();

        gizmo = null;
    }
}