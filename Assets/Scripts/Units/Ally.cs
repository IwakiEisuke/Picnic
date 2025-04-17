using System.Collections;
using System.Linq;
using UnityEngine;

public class Ally : UnitBase
{
    private UnitMoveLogicBase moveLogic = new AllyMoveLogic();
    protected override UnitMoveLogicBase MoveLogic => moveLogic;
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