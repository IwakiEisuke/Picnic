using System.Linq;
using UnityEngine;

/// <summary>
/// 弾を飛ばす攻撃
/// </summary>
[CreateAssetMenu(fileName = "ProjectileAttackAction", menuName = "Actions/ProjectileAttackAction")]
public class ProjectileAttackAction : ActionBase
{
    [SerializeField] float baseAttackRange = 3;
    [SerializeField] float bulletRange = 5;
    [SerializeField] bool penetration;
    [SerializeField] float speed = 10f;
    [SerializeField] float angle = 5;
    [SerializeField] Vector3 projectileOffset;
    [SerializeField] GameObject projectilePref;
    [SerializeField] AttackData baseAttackData;

    Vector3 targetPos;

    float Damage => level * (baseAttackData.damage + _status.atk);
    float AttackTriggerRange => baseAttackRange + _status.attackRadius;
    float BulletRange => bulletRange + baseAttackRange + _status.attackRadius;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, AttackTriggerRange, _parent.EntityType, opponent, selfInclude, out var target))
        {
            targetPos = target.transform.position;
            return Damage / interval;
        }

        return -1;
    }

    public override ActionExecuteInfo Execute()
    {
        // 弾のプレハブを生成
        var rot = Quaternion.LookRotation(targetPos - transform.position) * Quaternion.Euler(0, Random.Range(-angle, angle), 0);
        var obj = Instantiate(projectilePref, transform.position + projectileOffset, rot);
        obj.GetComponent<AttackCollider>().data = new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects);
        obj.layer = _parent.Manager.GetEntityLayer(_parent, !opponent);
        var projectile = obj.GetComponent<ProjectileController>();
        projectile.lifeRange = BulletRange;

        if (obj.TryGetComponent<DestroyOnHit>(out var destroyOnHit))
        {
            destroyOnHit.enabled = !penetration; // 貫通しない攻撃なら有効
            if (!opponent) // 味方を対象とする場合、自身に衝突しても破壊されないように設定
            {
                destroyOnHit.ignoreTargets = new[] { _parent.transform };
            }
        }
        projectile.speed = speed;
        // その場に留まらせる
        _agent.SetDestination(transform.position);

        return new ActionExecuteInfo(true, this, interval, loopCount, loopInterval);
    }
}
