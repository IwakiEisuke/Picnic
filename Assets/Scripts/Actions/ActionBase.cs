using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class ActionBase : ScriptableObject
{
    protected UnitBase _parent;
    protected NavMeshAgent _agent;
    protected UnitStats _stats;
    protected AttackController _attackController;

    readonly Collider[] _hits = new Collider[10]; // OverlapSphereの結果を格納する配列

    public void Initialize(UnitBase parent)
    {
        _parent = parent;
        _agent = parent.Agent;
        _stats = parent.Stats;
        _attackController = parent.GetComponent<AttackController>();
    }

    public abstract float Evaluate();
    public abstract void Execute();

    protected Transform[] CheckAround(Vector3 position, float radius, LayerMask layerMask)
    {
        var hitCount = Physics.OverlapSphereNonAlloc(position, radius, _hits, layerMask.value);
        // 主要コンポーネントのアタッチされているTransformを取得するためRigidbodyが存在する場合はRigidbodyのTransformを、そうでない場合はColliderのTransformを使用
        var rootTransforms = _hits.Select(c => c.attachedRigidbody != null ? c.attachedRigidbody.transform : c.transform);
        var array = rootTransforms.Take(hitCount).OrderBy(c => (c.transform.position - position).sqrMagnitude).ToArray();
        return array;
    }
}
