using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Effects/HealEffect")]
public class HealEffect : StatusEffectAssetBase
{
    [SerializeField] float effectValue = 1;

    public override void Apply(UnitGameStatus target)
    {
        target.owner.Health.Heal(effectValue);
    }

    public override void SetCancelCondition(UnitGameStatus status, StatusEffector effector)
    {
        status.owner.HitManager.OnAttacked += info => effector.Cancel();
    }
}