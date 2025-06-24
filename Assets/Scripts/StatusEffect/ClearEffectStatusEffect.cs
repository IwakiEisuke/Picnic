using UnityEngine;

[CreateAssetMenu(fileName = "ClearEffectStatusEffect", menuName = "Effects/ClearEffectStatusEffect")]
public class ClearEffectStatusEffect : StatusEffectAssetBase
{
    [SerializeField] int maxClearCount = 1;
    [SerializeField] EffectType clearEffectType;

    public override void Apply(UnitGameStatus target)
    {
        target.owner.StatusEffectManager.ClearEffects(clearEffectType, maxClearCount);
    }

    public override float Evaluate(UnitBase unit)
    {
        return unit.StatusEffectManager.GetEffects(clearEffectType).Count;
    }
}