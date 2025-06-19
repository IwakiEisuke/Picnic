using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Actions/MeleeAttack")]
public class MeleeAttackAction : ActionBase
{
    public AttackData attackData;

    Transform _target;

    public override float Evaluate()
    {
        var targets = GetOverlapSphere(_parent.transform.position, _status.attackRadius, _parent.opponentLayer);

        if (targets.Length == 0)
        {
            return -1f;
        }
        else
        {
            _target = targets[0];
            return level * _status.atk;
        }
    }

    public override ActionExecuteInfo Execute()
    {
        _attackController.AttackDirectly(_target, attackData);
        return new ActionExecuteInfo(true, this, interval);
    }
}
