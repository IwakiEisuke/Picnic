using UnityEngine;

[CreateAssetMenu(fileName = "DashEffect", menuName = "Effects/DashEffect")]
public class DashEffect : StatusEffectAssetBase
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
}