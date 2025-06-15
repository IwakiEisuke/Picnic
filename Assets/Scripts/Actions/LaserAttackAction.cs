using UnityEngine;

/// <summary>
/// 直線状の即時着弾貫通攻撃
/// </summary>
[CreateAssetMenu(fileName = "LaserAttackAction", menuName = "Actions/LaserAttackAction")]
public class LaserAttackAction : ActionBase
{
    [SerializeField, Range(1, 3)] int level = 1;

    [SerializeField] float baseAttackRange = 20;
    [SerializeField] float laserRadius = 3;

    [SerializeField] AttackData baseAttackData;

    float Damage => level * (baseAttackData.damage + _stats.Atk);
    float AttackRange => level * baseAttackRange + _stats.AttackRadius;

    Transform[] targets;
    Vector3 laserDirection;

    public override float Evaluate()
    {
        targets = CheckAround(transform.position, AttackRange, _parent.opponentLayer);

        if (targets.Length != 0)
        {
            laserDirection = (targets[0].position - transform.position).normalized;

            var hitCount = LaserCast(transform.position, laserDirection, AttackRange, laserRadius, _parent.opponentLayer);

            return Damage * hitCount / interval;
        }
        else
        {
            return -1f;
        }
    }

    public override ActionExecuteInfo Execute()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            _attackController.AttackDirectly(targets[i], new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime));
        }
        return new ActionExecuteInfo(true, this, interval);
    }

    public override void Update()
    {
        Debug.Log("Update Laser Action");
    }
}
