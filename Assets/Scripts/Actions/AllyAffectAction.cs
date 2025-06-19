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
        if (TryGetNearestAround(transform.position, _status.attackRadius, LayerMask.GetMask(LayerMask.LayerToName(_parent.gameObject.layer)), out var target))
        {
            _target = target;
            return EffectValue / interval;
        }

        return -1;
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