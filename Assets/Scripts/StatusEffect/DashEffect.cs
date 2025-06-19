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

    public override float Evaluate(UnitBase unit)
    {
        // ÀÛ‚É‚ÍAction‚Ì”­“®”ÍˆÍ‚É“ü‚é‚Ü‚Å‚ªˆÚ“®ŠÔ‚È‚Ì‚ÅA‰ß‘å•]‰¿‚³‚ê‚Ä‚¢‚é
        var remainTime = unit.Agent.remainingDistance / (unit.Status.speed + effectValue);
        return Mathf.Min(1, remainTime / _duration);
    }
}