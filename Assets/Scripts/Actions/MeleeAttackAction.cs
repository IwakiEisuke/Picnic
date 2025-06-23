using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Actions/MeleeAttack")]
public class MeleeAttackAction : ActionBase
{
    public AttackData baseAttackData;

    EntityBase _target;

    int Damage => baseAttackData.damage * level + _status.atk;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, _status.attackRadius, _parent.EntityType, opponent, selfInclude, out _target))
        {
            return level * _status.atk;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        _attackController.AttackDirectly(_target.transform, new AttackData(baseAttackData.id, Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects));
        return new ActionExecuteInfo(true, this, interval);
    }
}
