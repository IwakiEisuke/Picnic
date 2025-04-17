using UnityEngine;
using UnityEngine.AI;

public abstract class UnitMoveLogicBase
{
    protected NavMeshAgent _agent;
    protected UnitStats _stats;
    protected Collider[] _hits = new Collider[1];

    protected UnitBase parent;
    protected bool isInitialized;

    public void Init(UnitBase parent)
    {
        this.parent = parent;
        _agent = parent.GetComponent<NavMeshAgent>();
        _stats = parent.Stats;
        isInitialized = true;
    }

    protected bool IsInitialized()
    {
        if (isInitialized)
        {
            return true;
        }

        Debug.Log("Is not Initialized");
        return false;
    }

    public void Attack()
    {
        Debug.Log($"{_agent.name}: Attack");
        foreach (var damageable in _hits[0].GetComponentsInParent<IDamageable>())
        {
            damageable.TakeDamage(_stats);
        }
    }

    public bool CheckAround(LayerMask layerMask)
    {
        return Physics.OverlapSphereNonAlloc(_agent.transform.position, _stats.AttackRadius, _hits, layerMask.value) > 0;
    }

    public abstract void Start();
}