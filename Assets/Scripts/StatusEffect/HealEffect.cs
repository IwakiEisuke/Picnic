using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Effects/HealEffect")]
public class HealEffect : StatusEffectAssetBase
{
    [SerializeField] float effectValue = 1;

    public override void Apply(UnitGameStatus target)
    {
        target.owner.Health.Heal(effectValue);
    }

    public override float Evaluate(UnitBase unit)
    {
        return 1 - unit.Health.HealthRatio;
    }
}