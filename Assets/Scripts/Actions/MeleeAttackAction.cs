using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Actions/MeleeAttack")]
public class MeleeAttackAction : ActionBase
{
    public AttackData attackData;

    EntityBase _target;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(transform.position, _status.attackRadius, _parent.EntityType, opponent, out _target))
        {
            return level * _status.atk;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        _attackController.AttackDirectly(_target.transform, attackData);
        return new ActionExecuteInfo(true, this, interval);
    }
}
