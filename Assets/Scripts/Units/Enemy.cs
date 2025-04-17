using UnityEngine;
using UnityEngine.AI;

public class Enemy : UnitBase
{
    public FSM movementFSM;
    public FSM attackFSM;
    public void Awake()
    {
        movementFSM =
            new(
                new()
                {
                    { new MoveToOrigin(this), new(){ new FSM.Transition(1, () => false) } },
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

public class MoveToOrigin : FSM.IState
{
    NavMeshAgent _agent;

    public MoveToOrigin(UnitBase target) { _agent = target.GetComponent<NavMeshAgent>(); }

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        _agent.stoppingDistance = 0;
        _agent.SetDestination(Vector3.zero);
    }
}
