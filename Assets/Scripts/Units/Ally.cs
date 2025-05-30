using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Ally : UnitBase
{
    private NavMeshAgent agent;

    public FSM2 movementFSM;
    public FSM attackFSM;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        movementFSM =
            new(
                new()
                {
                    { new NearTargetMove(this) },
                    { new GoToClickPos(this) },
                }
            );

        attackFSM =
            new(
                new()
                {
                    { new NearTargetAttack(this), new(){ new FSM.Transition(3, () => false) } },
                }
            );
    }

    private void Update()
    {
        movementFSM.Update();
        attackFSM.Update();
    }

    public void MoveToNearestTarget()
    {
        movementFSM.Next(0); // NearTargetMove state
    }

    public void MoveToClickPos()
    {
        movementFSM.Next(1); // GoToClickPos state
    }
}

/// <summary>
/// 最も近い目標に向かって移動する
/// </summary>
public class NearTargetMove : FSM.IState
{
    readonly UnitBase _parent;
    readonly UnitStats _stats;
    readonly NavMeshAgent _agent;

    Vector3 _initPos;

    public NearTargetMove(UnitBase parent)
    {
        _parent = parent;
        _stats = parent.Stats;
        _agent = parent.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        _initPos = Quaternion.AngleAxis(90, Vector3.right) * Random.insideUnitCircle;
    }

    public void Exit() { }

    public void Update()
    {
        if (_stats.isSortie)
        {
            var targets = GameObject.FindGameObjectsWithTag(_parent.destinationTag);
            if (targets.Count() > 0)
            {
                var closest = targets.OrderBy(x => Vector3.Distance(_agent.transform.position, x.transform.position)).First();
                _agent.stoppingDistance = _stats.AttackRadius;
                _agent.SetDestination(closest.transform.position);
            }
            else
            {
                _agent.SetDestination(_initPos);
            }
        }
        else
        {
            _agent.stoppingDistance = 0;
            _agent.SetDestination(Vector3.zero);
        }
    }
}

/// <summary>
/// 最も近い目標を攻撃する
/// </summary>
public class NearTargetAttack : FSM.IState
{
    readonly UnitBase _parent;
    readonly UnitStats _stats;
    readonly NavMeshAgent _agent;
    readonly Collider[] _hits = new Collider[1];

    float t;

    public NearTargetAttack(UnitBase parent)
    {
        _parent = parent;
        _stats = parent.Stats;
        _agent = parent.GetComponent<NavMeshAgent>();
    }

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        t -= Time.deltaTime;

        if (t <= 0 && CheckAround(_parent.opponentLayer))
        {
            Attack();
            t = _stats.AttackInterval;
        }
    }

    private void Attack()
    {
        Debug.Log($"{_agent.name}: Attack");
        foreach (var damageable in _hits[0].GetComponentsInParent<IDamageable>())
        {
            damageable.TakeDamage(_stats);
        }
    }

    private bool CheckAround(LayerMask layerMask)
    {
        return Physics.OverlapSphereNonAlloc(_agent.transform.position, _stats.AttackRadius, _hits, layerMask.value) > 0;
    }
}


public class GoToClickPos : FSM.IState
{
    readonly UnitBase _parent;
    readonly UnitStats _stats;
    readonly NavMeshAgent _agent;

    public GoToClickPos(UnitBase parent)
    {
        _parent = parent;
        _stats = parent.Stats;
        _agent = parent.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            _agent.SetDestination(hit.point);
        }
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}