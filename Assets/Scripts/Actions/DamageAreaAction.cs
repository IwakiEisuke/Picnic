using UnityEngine;

/// <summary>
/// 持続ダメージエリア
/// </summary>
[CreateAssetMenu(fileName = "DamageAreaAction", menuName = "Actions/DamageAreaAction")]
public class DamageAreaAction : ActionBase
{
    [SerializeField] float baseAttackRange = 5;
    [SerializeField] float damageAreaRadius = 3;
    [SerializeField] float duration = 3;
    [SerializeField] bool positionScattering;
    [SerializeField] float scatterRadius = 1.5f;
    [SerializeField] AttackData baseAttackData;
    [SerializeField] bool canMoveWhileExecuting;
    [SerializeField] Vector3 offset;

    [SerializeField] GameObject damageAreaPrefab;

    readonly float[] damageMultipliers = { 1, 1.1f, 1.3f };
    readonly float[] rangeMultipliers = { 1, 1.2f, 1.5f };

    Vector3 targetPosition;

    float Damage => damageMultipliers[level] * (baseAttackData.damage + _status.atk);
    float AttackRange => rangeMultipliers[level] * (baseAttackRange + _status.attackRadius);

    public override float Evaluate()
    {
        if (TryGetNearestAround(transform.position, AttackRange, _parent.opponentLayer, out var target))
        {
            var targets = GetOverlapSphere(target.position, damageAreaRadius, _parent.opponentLayer);
            targetPosition = target.position;
            return Damage * targets.Length / interval;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        if (!canMoveWhileExecuting) _agent.SetDestination(transform.position);

        var obj = Instantiate(damageAreaPrefab, transform.position + offset, Quaternion.identity);
        obj.layer = transform.gameObject.layer;
        
        if (obj.TryGetComponent<ITargetedObject>(out var targetedObject))
        {
            var targetPos = targetPosition;
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
            areaObject.InitializeArea(damageAreaRadius, duration);
        }
        else
        {
            Debug.LogWarning($"{name}: {nameof(DamageAreaAction)}から非{nameof(IAreaObject)}なインスタンスを生成しました。{nameof(IAreaObject)}コンポーネントをアタッチしたオブジェクトを入れてください");
        }

        Destroy(obj, duration);
        return new ActionExecuteInfo(true, this, interval, loopCount, loopInterval);
    }
}
