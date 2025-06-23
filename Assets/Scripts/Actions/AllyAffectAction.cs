using System.Linq;
using UnityEngine;

/// <summary>
/// 弾を飛ばす攻撃
/// </summary>
[CreateAssetMenu(fileName = "AllyAffectAction", menuName = "Actions/AllyAffectAction")]
public class AllyAffectAction : ActionBase
{
    [SerializeField] StatusEffectAssetBase[] statusEffects;

    EntityBase _target;

    float EffectValue => _status.atk * level;

    public override float Evaluate()
    {
        var targets = _parent.Manager.GetEntityAround(_parent, transform.position, _status.attackRadius, _parent.EntityType, opponent, selfInclude).ToArray();

        var maxScore = -1f;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null || !targets[i].TryGetComponent<StatusEffectManager>(out _)) continue;

            var score = 0f;
            for (int j = 0; j < statusEffects.Length; j++)
            {
                if (targets[i].TryGetComponent<UnitBase>(out var unit))
                {
                    score += statusEffects[j].Evaluate(unit);
                }
            }

            if (score > maxScore)
            {
                _target = targets[i];
                maxScore = score;
            }
        }

        return maxScore / cooldownTime;
    }

    public override ActionExecuteInfo Execute()
    {
        if (_target == null)
        {
            return new ActionExecuteInfo(false, this);
        }

        if (_target.TryGetComponent<StatusEffectManager>(out var statusEffectManager))
        {
            statusEffectManager.AddEffect(statusEffects);
            return new ActionExecuteInfo(true, this);
        }

        return new ActionExecuteInfo(false, this);
    }
}