using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Ally : UnitBase
{
    public FSM movementFSM;
    public FSM attackFSM;

    public void Awake()
    {
        movementFSM =
            new(
                new()
                {
                    { new NearTargetMove(this), new(){ new FSM.Transition(1, () => false) } },
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
}

/// <summary>
/// Å‚à‹ß‚¢–Ú•W‚ÉŒü‚©‚Á‚ÄˆÚ“®‚·‚é
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
/// Å‚à‹ß‚¢–Ú•W‚ğUŒ‚‚·‚é
/// </summary>
public class NearTargetAttack : FSM.IState
{
    readonly UnitBase _parent;
    readonly UnitStats _stats;
    readonly NavMeshAgent _agent;
    readonly Collider[] _hits;

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

class AllyMoveLogic : UnitMoveLogicBase
{
    public override void Start()
    {
        if (!IsInitialized()) return;

        parent.StartCoroutine(MoveState());
        parent.StartCoroutine(AttackState());

        parent.Health.OnDestroyEvent += () => parent.StopAllCoroutines();
    }

    protected IEnumerator MoveState()
    {
        var pos = Quaternion.AngleAxis(90, Vector3.right) * Random.insideUnitCircle;

        while (true)
        {
            if (_stats.isSortie)
            {
                var targets = GameObject.FindGameObjectsWithTag(parent.destinationTag);
                if (targets.Count() > 0)
                {
                    var closest = targets.OrderBy(x => Vector3.Distance(_agent.transform.position, x.transform.position)).First();
                    _agent.stoppingDistance = _stats.AttackRadius;
                    _agent.SetDestination(closest.transform.position);
                }
                else
                {
                    _agent.SetDestination(pos);
                }
            }
            else
            {
                _agent.stoppingDistance = 0;
                _agent.SetDestination(Vector3.zero);
            }

            yield return null;
        }
    }

    protected IEnumerator AttackState()
    {
        while (true)
        {
            if (CheckAround(parent.opponentLayer))
            {
                Attack();
                yield return new WaitForSeconds(_stats.AttackInterval);
            }
            else
            {
                yield return null;
            }
        }
    }
}