using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ally : UnitBase
{
    public FSM2<State> movementFSM;
    public FSM attackFSM;

    private void Start()
    {
        movementFSM =
            new(
                new()
                {
                    { State.MoveToNearestTarget, new NearTargetMove(this) },
                    { State.MoveToClickPos, new GoToClickPos(this) },
                    { State.Follow, new FollowTarget(this) },
                    { State.Stop, new Stop(this) },
                    { State.MoveToHive, new MoveToHive(this) },
                    { State.InteractTarget, new InteractTarget(this) },
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

    public void Next(State type)
    {
        movementFSM.Next(type);
    }

    public enum State
    {
        MoveToNearestTarget,
        MoveToClickPos,
        Follow,
        Stop,
        MoveToHive,
        InteractTarget
    }
}

/// <summary>
/// ç≈Ç‡ãﬂÇ¢ñ⁄ïWÇ…å¸Ç©Ç¡Çƒà⁄ìÆÇ∑ÇÈ
/// </summary>
public class NearTargetMove : FSMState
{
    Vector3 _initPos;

    public NearTargetMove(UnitBase parent) : base(parent)
    {
    }

    public override void Enter()
    {
        _initPos = Quaternion.AngleAxis(90, Vector3.right) * Random.insideUnitCircle;
    }

    public override void Update()
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
/// ç≈Ç‡ãﬂÇ¢ñ⁄ïWÇçUåÇÇ∑ÇÈ
/// </summary>
public class NearTargetAttack : FSMState
{
    readonly Collider[] _hits = new Collider[1];

    float t;

    public NearTargetAttack(UnitBase parent) : base(parent)
    {
    }

    public override void Update()
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
        Log("Attack");
        foreach (var damageable in _hits[0].GetComponentsInParent<Health>())
        {
            damageable.TakeDamage(_stats);
        }
    }

    private bool CheckAround(LayerMask layerMask)
    {
        return Physics.OverlapSphereNonAlloc(_agent.transform.position, _stats.AttackRadius, _hits, layerMask.value) > 0;
    }
}


public class GoToClickPos : FSMState
{
    public GoToClickPos(UnitBase parent) : base(parent)
    {
    }

    public override void Enter()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            _agent.SetDestination(hit.point);
        }
    }
}

public class FollowTarget : FSMState
{
    Transform _target;

    public FollowTarget(UnitBase parent) : base(parent)
    {
    }

    public override void Enter()
    {
        _target = Object.FindAnyObjectByType<UnitSelector>().ControlTarget;
    }

    public override void Update()
    {
        _parent.Agent.SetDestination(_target.position);
    }
}

public class Stop : FSMState
{
    public Stop(UnitBase parent) : base(parent)
    {
    }

    public override void Enter()
    {
        _parent.Agent.isStopped = true;
    }
}

public class MoveToHive : FSMState
{
    public MoveToHive(UnitBase parent) : base(parent)
    {
    }

    public override void Enter()
    {
        var hive = GameObject.Find("Hive");
        if (hive != null)
        {
            _parent.Agent.SetDestination(hive.transform.position);
        }
        else
        {
            Log("Hive not found, setting destination to zero.");
            _parent.Agent.SetDestination(Vector3.zero);
        }
    }
}

public class InteractTarget : FSMState
{
    Transform _target;
    IInteractable _interactable;
    readonly Timer _timer = new();

    public InteractTarget(UnitBase parent) : base(parent)
    {
    }

    public override void Enter()
    {
        _target = Object.FindAnyObjectByType<UnitSelector>().ControlTarget;
        _interactable = _target.GetComponent<IInteractable>();
        if (_interactable != null)
        {
            _timer.TimeUp += _interactable.Interact;
            _interactable.CancelInteract += Cancel;
            _parent.Agent.SetDestination(_target.position);
        }
        else
        {
            Log("No target to interact with");
            Cancel();
        }
    }

    public override void Exit()
    {
        _timer.Cancel();
        if (_interactable != null)
        {
            _timer.TimeUp -= _interactable.Interact;
            _interactable.CancelInteract -= Cancel;
        }
    }

    public override void Update()
    {
        if (_interactable == null) return;

        if (Vector3.Distance(_target.position, _parent.transform.position) < _parent.Stats.AttackRadius)
        {
            if (!_timer.IsActive) _timer.Set(_interactable.Duration);
        }
    }

    private void Cancel()
    {
        if (_parent is Ally ally)
        {
            ally.movementFSM.Next(Ally.State.MoveToNearestTarget);
        }
    }
}