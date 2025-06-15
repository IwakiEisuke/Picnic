using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Actions/MeleeAttack")]
public class MeleeAttackAction : ActionBase
{
    [Range(1, 3)] public int level = 1;
    public AttackData attackData;

    Transform _target;

    public override float Evaluate()
    {
        var targets = CheckAround(_parent.transform.position, _stats.AttackRadius, _parent.opponentLayer);

        if (targets.Length == 0)
        {
            return 0f;
        }
        else
        {
            _target = targets[0];
            return level * _stats.Atk;
        }
    }

    public override ActionExecuteInfo Execute()
    {
        _attackController.AttackDirectly(_target, attackData);
        return new ActionExecuteInfo(true, this, interval);
    }
}
