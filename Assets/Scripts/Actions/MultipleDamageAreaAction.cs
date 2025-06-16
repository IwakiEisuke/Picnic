using UnityEngine;

/// <summary>
/// 散乱持続ダメージエリア
/// </summary>
[CreateAssetMenu(fileName = "MultipleDamageAreaAction", menuName = "Actions/MultipleDamageAreaAction")]
public class MultipleDamageAreaAction : ActionBase
{
    [SerializeField] float baseAttackRange = 5;
    [SerializeField] float damageAreaRadius = 3;
    [SerializeField] float duration = 3;
    [SerializeField] float scatterRadius = 1.5f;
    [SerializeField] AttackData baseAttackData;

    [SerializeField] GameObject damageAreaPrefab;

    readonly float[] damageMultipliers = { 1, 1.1f, 1.3f };
    readonly float[] rangeMultipliers = { 1, 1.2f, 1.5f };

    Vector3 targetPosition;

    float Damage => damageMultipliers[level] * (baseAttackData.damage + _stats.Atk);
    float AttackRange => rangeMultipliers[level] * (baseAttackRange + _stats.AttackRadius);

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
        _agent.SetDestination(transform.position);

        var scatteredPos = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * scatterRadius;
        var obj = Instantiate(damageAreaPrefab, targetPosition + scatteredPos, Quaternion.identity);
        obj.layer = transform.gameObject.layer;
        obj.GetComponent<AttackCollider>().data = new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime);
        Destroy(obj, duration);
        return new ActionExecuteInfo(true, this, interval, loopCount, loopInterval);
    }
}
