using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Actions/MeleeAttack")]
public class MeleeAttackAction : ActionBase
{
    [Range(1, 3)] public int level = 1;
    public AttackData attackData;

    public override float Evaluate()
    {
        var targets = CheckAround(_parent.transform.position, _stats.AttackRadius, _parent.opponentLayer);

        if (targets.Length == 0)
        {
            return 0f; // 攻撃可能なターゲットがいない場合は評価値0
        }
        else
        {
            return level * _stats.Atk; // 攻撃可能なターゲットがいる場合はレベルに応じた評価値を返す
        }
    }

    public override void Execute()
    {
        var targets = CheckAround(_parent.transform.position, _stats.AttackRadius, _parent.opponentLayer);
        if (targets.Length > 0)
        {
            // 最も近いターゲットを取得
            var closestTarget = targets[0];
            if (closestTarget != null)
            {
                // 攻撃を実行
                _attackController.AttackDirectly(closestTarget.transform, attackData);
            }
        }
    }
}
