using UnityEngine;

/// <summary>
/// 一定時間の間ユニットの移動速度を増減させるステータス効果
/// </summary>
[CreateAssetMenu(fileName = "SpeedModifierEffect", menuName = "Effects/SpeedModifierEffect")]
public class SpeedModifierEffect : StatusEffectAssetBase
{
    [SerializeField] float effectValue = 1;

    public override void Apply(UnitGameStatus target)
    {
        target.speed += effectValue;
    }

    public override float Evaluate(UnitBase unit)
    {
        // 実際にはActionの発動範囲に入るまでが移動時間なので、過大評価されている
        var remainTime = unit.Agent.remainingDistance / (unit.Status.speed + effectValue);
        return Mathf.Min(1, remainTime / _duration);
    }
}