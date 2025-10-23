using UnityEngine;

[CreateAssetMenu(fileName = "OnAttackedCancelCondition", menuName = "Effects/CancelConditions/OnAttackedCancelCondition", order = 0)]
public class OnAttackedCancelCondition : EffectCancelConditionBase
{
    public override bool IsCancel(UnitGameStatus status)
    {
        return status.owner.HitManager.IsAttacked;
    }
}
