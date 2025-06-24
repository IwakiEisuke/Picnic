using UnityEngine;

/// <summary>
/// 一定時間の間ユニットのステータスを増減させるステータス効果
/// </summary>
[CreateAssetMenu(fileName = "StatusModifierEffect", menuName = "Effects/StatusModifierEffect")]
public class StatusModifierEffect : StatusEffectAssetBase
{
    [SerializeField] int maxHealth;
    [SerializeField] float speed;
    [SerializeField] int atk;
    [SerializeField] float attackInterval;
    [SerializeField] float attackRadius;
    [SerializeField] float knockBack;
    [SerializeField] float damageReflect;
    [SerializeField] float resistance;
    [SerializeField] float effectEvaluateScore ;

    public override void Apply(UnitGameStatus target)
    {
        target.maxHealth += maxHealth;
        target.speed += speed;
        target.atk += atk;
        target.attackInterval += attackInterval;
        target.attackRadius += attackRadius;
        target.knockBack += knockBack;
        target.damageReflect += damageReflect;
        target.resistance += resistance;
    }

    public override float Evaluate(UnitBase unit)
    {
        return effectEvaluateScore;
    }
}