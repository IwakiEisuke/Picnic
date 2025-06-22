using System.Linq;
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

    float Damage => damageMultipliers[level] * (baseAttackData.damage + _status.atk);
    Vector3 TriggerPos => transform.position + transform.forward + transform.rotation * offset;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(transform.position, attackRange, _parent.EntityType, opponent, out _))
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
        if (_parent.Manager.TryGetNearestEntityAround(TriggerPos, triggerRadius, _parent.EntityType, opponent, out _))
        {
            var targets = _parent.Manager.GetEntityAround(TriggerPos, impactRadius, _parent.EntityType, opponent);
            if (targets.Count() > 0)
            {
                _attackController.AttackDirectly(targets.Select(x => x.transform).ToArray(), new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects));
                _agent.SetDestination(transform.position);
            }
        }
    }
}
