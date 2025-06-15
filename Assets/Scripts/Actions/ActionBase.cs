using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class ActionBase : ScriptableObject
{
    [SerializeField] protected float interval; // アクションの実行間隔

    protected UnitBase _parent;
    protected NavMeshAgent _agent;
    protected UnitStats _stats;
    protected AttackController _attackController;
    protected Transform transform;

    readonly Collider[] _hits = new Collider[10]; // OverlapSphereの結果を格納する配列

    public void Initialize(UnitBase parent)
    {
        _parent = parent;
        _agent = parent.Agent;
        _stats = parent.Stats;
        _attackController = parent.GetComponent<AttackController>();
        transform = parent.transform;
    }

    public abstract float Evaluate();
    public abstract ActionExecuteInfo Execute();

    protected Transform[] CheckAround(Vector3 position, float radius, LayerMask layerMask)
    {
        var hitCount = Physics.OverlapSphereNonAlloc(position, radius, _hits, layerMask.value);
        // 主要コンポーネントのアタッチされているTransformを取得するためRigidbodyが存在する場合はRigidbodyのTransformを、そうでない場合はColliderのTransformを使用
        var rootTransforms = _hits.Select(c => c.attachedRigidbody != null ? c.attachedRigidbody.transform : c.transform);
        var array = rootTransforms.Take(hitCount).OrderBy(c => (c.transform.position - position).sqrMagnitude).ToArray();
        return array;
    }
}

/// <summary>
/// アクションの実行結果を表す構造体
/// </summary>
public readonly struct ActionExecuteInfo
{
    public readonly bool success;
    public readonly ActionBase action;
    public readonly float interval;

    public ActionExecuteInfo(bool success, ActionBase action = null, float interval = 0f)
    {
        this.success = success;
        this.action = action;
        this.interval = interval;
    }

    public override readonly string ToString() => $"{action.name} (Interval: {interval})";
}