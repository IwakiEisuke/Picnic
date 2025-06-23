using UnityEngine;

/// <summary>
/// 直線状の即時着弾貫通攻撃
/// </summary>
[CreateAssetMenu(fileName = "LaserAttackAction", menuName = "Actions/LaserAttackAction")]
public class LaserAttackAction : ActionBase
{
    [SerializeField] float baseAttackRange = 20;
    [SerializeField] float laserAdditionalRange = 5;
    [SerializeField] float laserRadius = 3;
    [SerializeField] float laserDuration = 1;

    [SerializeField] AttackData baseAttackData;

    float Damage => level * (baseAttackData.damage + _status.atk);
    float AttackRange => level * baseAttackRange + _status.attackRadius;
    float LaserRange => level * (baseAttackRange + laserAdditionalRange) + _status.attackRadius;

    Transform[] targets;
    Vector3 laserDirection;

    float laserEmittedTime;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, AttackRange, _parent.EntityType, opponent, selfInclude, out var target))
        {
            laserDirection = (target.transform.position - transform.position).normalized;
            targets = LaserCast(transform.position, laserDirection, LaserRange, laserRadius, _parent.opponentLayer);
            return Damage * targets.Length / cooldownTime;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        _attackController.AttackDirectly(targets, new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects));
        // その場に留める
        _agent.SetDestination(transform.position);
        // レーザー照射時間をリセット
        laserEmittedTime = 0;
        return new ActionExecuteInfo(true, this);
    }

    public override void Update()
    {
        if (laserEmittedTime < laserDuration)
        {
            laserEmittedTime += Time.deltaTime;
            targets = LaserCast(transform.position, laserDirection, LaserRange, laserRadius, _parent.opponentLayer);
            _attackController.AttackDirectly(targets, new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects));
        }
    }
}
