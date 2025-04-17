using System.Collections;
using UnityEngine;

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

public class EnemyMoveLogic : UnitMoveLogicBase
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
        while (true)
        {
            _agent.stoppingDistance = 0;
            _agent.SetDestination(Vector3.zero);
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