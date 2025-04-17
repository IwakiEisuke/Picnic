using System.Collections;
using UnityEngine;

public class Enemy : UnitBase
{
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