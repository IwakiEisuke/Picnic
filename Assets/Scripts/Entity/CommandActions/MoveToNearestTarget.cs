using System.Linq;
using UnityEngine;

namespace Entity.CommandActions
{
    [CreateAssetMenu(fileName = "MoveToNearestTarget", menuName = "ScriptableObjects/CommandActions/MoveToNearestTarget")]
    public class MoveToNearestTarget : CommandActionBase
    {
        private UnitBase unit;

        public override void Init(GameObject gameObject)
        {
            unit = gameObject.GetComponent<UnitBase>();
        }

        public override void UpdateAction()
        {
            var _agent = unit.Agent;
            var _stats = unit.Stats;

            var targets = GameObject.FindGameObjectsWithTag(unit.destinationTag);
            if (targets.Length > 0)
            {
                var closest = targets.OrderBy(x => Vector3.Distance(_agent.transform.position, x.transform.position)).First();
                _agent.stoppingDistance = _stats.AttackRadius;
                _agent.SetDestination(closest.transform.position);
            }
            else
            {
                _agent.SetDestination(unit.transform.position);
            }
        }
    }
}