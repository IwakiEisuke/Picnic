using UnityEngine;

/// <summary>
/// 近接範囲攻撃
/// </summary>
[CreateAssetMenu(fileName = "MeleeAreaAttackAction", menuName = "Actions/MeleeAreaAttackAction")]
public class MeleeAreaAttackAction : ActionBase
{
    [Range(1, 3)] public int level = 1;
    public AttackData baseAttackData;

    private Transform[] _targets;

    public override float Evaluate()
    {
        _targets = CheckAround(_parent.transform.position, _stats.AttackRadius, _parent.opponentLayer);

        if (_targets.Length == 0)
        {
            return 0f;
        }
        else
        {
            return level * baseAttackData.damage * _stats.Atk * _targets.Length; // ターゲットの数に応じて評価値を増加
        }
    }

    public override void Execute()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            var target = _targets[i];
            if (target != null)
            {
                _attackController.AttackDirectly(target, new AttackData(baseAttackData.id, baseAttackData.damage * _stats.Atk * level, baseAttackData.invincibleTime));
            }
        }
    }
}
