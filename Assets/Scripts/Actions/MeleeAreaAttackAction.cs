using System.Linq;
using UnityEngine;

/// <summary>
/// 近接範囲攻撃
/// </summary>
[CreateAssetMenu(fileName = "MeleeAreaAttackAction", menuName = "Actions/MeleeAreaAttackAction")]
public class MeleeAreaAttackAction : ActionBase
{
    public AttackData baseAttackData;

    private Transform[] _targets;

    public override float Evaluate()
    {
        _targets = _parent.Manager.GetEntityAround(_parent, transform.position, _status.attackRadius, _parent.EntityType, opponent, selfInclude).Select(x => x.transform).ToArray();

        if (_targets.Length == 0)
        {
            return 0f;
        }
        else
        {
            return level * baseAttackData.damage * _status.atk * _targets.Length; // ターゲットの数に応じて評価値を増加
        }
    }

    public override ActionExecuteInfo Execute()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            _attackController.AttackDirectly(_targets[i], new AttackData(baseAttackData.id, baseAttackData.damage * _status.atk * level, baseAttackData.invincibleTime, baseAttackData.statusEffects));
        }

        return new ActionExecuteInfo(true, this);
    }
}
