using System.Collections;
using UnityEngine;

public class Enemy : UnitBase
{
    protected override IEnumerator MoveState()
    {
        while (true)
        {
            _agent.stoppingDistance = 0;
            _agent.SetDestination(Vector3.zero);
            yield return null;
        }
    }

    protected override IEnumerator AttackState()
    {
        return base.AttackState();
    }
}
