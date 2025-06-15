using System;
using UnityEngine;
using UnityEngine.AI;

public class ActionBase : ScriptableObject
{
    protected UnitBase _parent;
    protected NavMeshAgent _agent;
    protected UnitStats _stats;
    protected AttackController _attackController;

    readonly Collider[] _hits = new Collider[10]; // OverlapSphere‚ÌŒ‹‰Ê‚ðŠi”[‚·‚é”z—ñ

    public void Initialize(UnitBase parent)
    {
        _parent = parent;
        _agent = parent.Agent;
        _stats = parent.Stats;
        _attackController = parent.GetComponent<AttackController>();
    }

    public virtual float Evaluate() => 0f;
    public virtual void Execute() { }

    protected Span<Collider> CheckAround(Vector3 position, float radius, LayerMask layerMask)
    {
        Physics.OverlapSphereNonAlloc(position, radius, _hits, layerMask.value);
        Array.Sort(_hits);
        return _hits.AsSpan();
    }
}
