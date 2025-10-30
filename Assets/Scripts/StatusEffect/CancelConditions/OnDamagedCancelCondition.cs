using UnityEngine;

[CreateAssetMenu(fileName = "OnDamagedCancelCondition", menuName = "Effects/CancelConditions/OnDamagedCancelCondition", order = 0)]
public class OnDamagedCancelCondition : EffectCancelConditionBase
{
    public override bool IsCancel(UnitGameStatus status)
    {
        return status.owner.HitManager.IsDamaged;
    }
}
