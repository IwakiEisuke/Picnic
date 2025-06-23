using UnityEngine;

/// <summary>
/// 何かを攻撃するまでの間、ユニットの移動速度を上昇させるステータス効果
/// </summary>
[CreateAssetMenu(fileName = "ChargeDashEffect", menuName = "Effects/ChargeDashEffect")]
public class ChargeDashEffect : StatusEffectAssetBase
{
    [SerializeField] float effectValue = 1;

    public override void Apply(UnitGameStatus target)
    {
        target.speed += effectValue;
    }

    public override void SetCancelCondition(UnitGameStatus status, StatusEffector effector)
    {
        status.owner.HitManager.OnAttacked += info => effector.Cancel();
    }

    public override float Evaluate(UnitBase unit)
    {
        // 実際にはActionの発動範囲に入るまでが移動時間なので、過大評価されている
        var remainTime = unit.Agent.remainingDistance / (unit.Status.speed + effectValue);
        return Mathf.Min(1, remainTime / _duration);
    }
}