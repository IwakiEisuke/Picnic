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
    [SerializeField] AttackData baseAttackData;

    [SerializeField] GameObject damageAreaPrefab;

    readonly float[] damageMultipliers = { 1, 1.1f, 1.3f};
    readonly float[] rangeMultipliers = { 1, 1.2f, 1.5f };

    Transform target;

    float Damage => damageMultipliers[level] * (baseAttackData.damage + _stats.Atk);
    float AttackRange => rangeMultipliers[level] * (baseAttackRange + _stats.AttackRadius);

    public override float Evaluate()
    {
        if (TryGetNearestAround(transform.position, AttackRange, _parent.opponentLayer, out target))
        {
            var targets = CheckAround(target.position, damageAreaRadius, _parent.opponentLayer);
            return Damage * targets.Length / interval;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        var obj = Instantiate(damageAreaPrefab, target.position, Quaternion.identity);
        obj.layer = transform.gameObject.layer;
        obj.GetComponent<AttackCollider>().data = new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime);
        Destroy(obj, duration);
        return new ActionExecuteInfo(true, this, interval);
    }
}
