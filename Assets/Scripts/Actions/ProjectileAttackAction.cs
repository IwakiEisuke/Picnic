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

    float Damage => level * (baseAttackData.damage + _stats.Atk);
    float AttackTriggerRange => baseAttackRange + _stats.AttackRadius;
    float BulletRange => bulletRange + baseAttackRange + _stats.AttackRadius;

    public override float Evaluate()
    {
        var targets = GetSortedOverlapSphere(transform.position, AttackTriggerRange, _parent.opponentLayer);

        if (targets.Length == 0)
        {
            return -1;
        }
        else
        {
            targetPos = targets[0].position;
            return Damage / interval;
        }
    }

    public override ActionExecuteInfo Execute()
    {
        // 弾のプレハブを生成
        var rot = Quaternion.LookRotation(targetPos - transform.position) * Quaternion.Euler(0, Random.Range(-angle, angle), 0);
        var obj = Instantiate(projectilePref, transform.position + projectileOffset, rot);
        obj.GetComponent<AttackCollider>().data = new AttackData(baseAttackData.id, (int)Damage, baseAttackData.invincibleTime, baseAttackData.statusEffects);
        // 生成元と衝突しないようにレイヤーを設定
        obj.layer = transform.gameObject.layer;
        var projectile = obj.GetComponent<ProjectileController>();
        projectile.lifeRange = BulletRange;
        projectile.destroyOnHit = !penetration;
        projectile.speed = speed;
        // その場に留まらせる
        _agent.SetDestination(transform.position);
        
        return new ActionExecuteInfo(true, this, interval, loopCount, loopInterval);
    }
}
