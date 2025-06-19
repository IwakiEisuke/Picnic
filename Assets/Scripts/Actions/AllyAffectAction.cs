using UnityEngine;

/// <summary>
/// 弾を飛ばす攻撃
/// </summary>
[CreateAssetMenu(fileName = "AllyAffectAction", menuName = "Actions/AllyAffectAction")]
public class AllyAffectAction : ActionBase
{
    [SerializeField] StatusEffectAssetBase[] statusEffects;

    Transform _target;

    float EffectValue => _status.atk * level;

    public override float Evaluate()
    {
        var targets = GetOverlapSphere(transform.position, _status.attackRadius, LayerMask.GetMask(LayerMask.LayerToName(_parent.gameObject.layer)));

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

        return maxScore / interval;
    }

    public override ActionExecuteInfo Execute()
    {
        if (_target == null)
        {
            return new ActionExecuteInfo(false, this, interval, loopCount, loopInterval);
        }

        if (_target.TryGetComponent<StatusEffectManager>(out var statusEffectManager))
        {
            statusEffectManager.AddEffect(statusEffects);
            return new ActionExecuteInfo(true, this, interval, loopCount, loopInterval);
        }

        return new ActionExecuteInfo(false, this, interval, loopCount, loopInterval);
    }
}