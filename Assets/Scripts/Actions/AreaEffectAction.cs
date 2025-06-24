using System.Linq;
using UnityEngine;

/// <summary>
/// 範囲効果
/// </summary>
[CreateAssetMenu(fileName = "AreaEffectAction", menuName = "Actions/AreaEffectAction")]
public class AreaEffectAction : ActionBase
{
    [SerializeField] float areaRadius = 3;
    [SerializeField] float areaDuration = 3;
    [SerializeField] bool positionScattering;
    [SerializeField] float scatterRadius = 1.5f;
    [SerializeField] AttackData baseAttackData;
    [SerializeField] bool canMoveWhileExecuting;
    [SerializeField] Vector3 offset;

    [SerializeField] GameObject damageAreaPrefab;

    readonly float[] damageMultipliers = { 1, 1.1f, 1.3f };
    readonly float[] rangeMultipliers = { 1, 1.2f, 1.5f };

    float Damage => damageMultipliers[level] * (baseAttackData.damage + _status.atk);
    float AreaEffectRange => rangeMultipliers[level] * (areaRadius + _status.attackRadius);

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, AreaEffectRange, _parent.EntityType, opponent, selfInclude, out var target))
        {
            var hitCount = _parent.Manager.GetEntityAround(_parent, target.transform.position, areaRadius, _parent.EntityType, opponent, selfInclude).Count();

            var damageScore = Damage * hitCount / cooldownTime;
            var effectsScore = baseAttackData.statusEffects.Sum(x => x.Evaluate(_parent));

            return damageScore + effectsScore;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        if (!canMoveWhileExecuting) _agent.SetDestination(transform.position);

        var obj = Instantiate(damageAreaPrefab, transform.position + offset, Quaternion.identity);
        obj.layer = _parent.Manager.GetEntityLayer(_parent, !opponent);

        if (obj.TryGetComponent<AttackCollider>(out var attackCollider))
        {
            if (!selfInclude) attackCollider.hitFilter.AddIgnore(transform);
        }

        if (obj.TryGetComponent<ITargetedObject>(out var targetedObject))
        {
            if (!selfInclude && obj.TryGetComponent<DestroyOnHit>(out var destroyOnHit))
            {
                destroyOnHit.ignoreTargets = new[] { _parent.transform };
            }

            var targetPos = transform.position;
            if (positionScattering)
            {
                targetPos += new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * scatterRadius;
            }
            targetedObject.InitializeTarget(targetPos, new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects));
        }
        else
        {
            Debug.LogWarning($"{name}: {nameof(DamageAreaAction)}から非{nameof(ITargetedObject)}なインスタンスを生成しました。{nameof(ITargetedObject)}コンポーネントをアタッチしたオブジェクトを入れてください");
        }

        if (obj.TryGetComponent<IAreaObject>(out var areaObject))
        {
            areaObject.InitializeArea(AreaEffectRange, areaDuration);
        }
        else
        {
            Debug.LogWarning($"{name}: {nameof(DamageAreaAction)}から非{nameof(IAreaObject)}なインスタンスを生成しました。{nameof(IAreaObject)}コンポーネントをアタッチしたオブジェクトを入れてください");
        }

        Destroy(obj, areaDuration);
        return new ActionExecuteInfo(true, this);
    }
}
