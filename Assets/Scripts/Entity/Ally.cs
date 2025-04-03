using System.Collections;
using System.Linq;
using UnityEngine;

public class Ally : UnitBase
{
    protected override IEnumerator AttackState()
    {
        return base.AttackState();
    }

    protected override IEnumerator MoveState()
    {
        var pos = Quaternion.AngleAxis(90, Vector3.right) * Random.insideUnitCircle;

        while (true)
        {
            if (stats.isSortie)
            {
                var targets = GameObject.FindGameObjectsWithTag(destinationTag);
                if (targets.Count() > 0)
                {
                    var closest = targets.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
                    _agent.stoppingDistance = stats.AttackRadius;
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
}
