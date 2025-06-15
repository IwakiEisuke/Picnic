using UnityEngine;

/// <summary>
/// 突進攻撃
/// </summary>
[CreateAssetMenu(fileName = "ChargeAttackAction", menuName = "Actions/ChargeAttackAction")]
public class ChargeAttackAction : ActionBase
{
    [SerializeField] float attackRange = 5f;
    [SerializeField] float triggerRadius = 0.5f;
    [SerializeField] float impactRadius = 2f;
    [SerializeField] StatusEffectAssetBase chargeEffect;
    [SerializeField] Vector3 offset;
    [SerializeField] AttackData baseAttackData;

    [SerializeField] float[] damageMultipliers = { 1, 1.5f, 2};

    Transform target;

    float Damage => damageMultipliers[level] * (baseAttackData.damage + _stats.Atk);
    Vector3 TriggerPos => transform.position + transform.forward + transform.rotation * offset;

    public override float Evaluate()
    {
        if (TryGetNearestAround(transform.position, attackRange, _parent.opponentLayer, out target))
        {
            return Damage / interval;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        _parent.StatusEffectManager.AddEffect(chargeEffect);
        return new ActionExecuteInfo(true, this, interval);
    }

    public override void Update()
    {
        if (TryGetNearestAround(TriggerPos, triggerRadius, _parent.opponentLayer, out _))
        {
            var targets = GetOverlapSphere(TriggerPos, impactRadius, _parent.opponentLayer);
            _attackController.AttackDirectly(targets, new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime));
        }
    }
}
