using UnityEngine;

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
        // ���ۂɂ�Action�̔����͈͂ɓ���܂ł��ړ����ԂȂ̂ŁA�ߑ�]������Ă���
        var remainTime = unit.Agent.remainingDistance / (unit.Status.speed + effectValue);
        return Mathf.Min(1, remainTime / _duration);
    }
}