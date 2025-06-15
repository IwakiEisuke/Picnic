using UnityEngine;

/// <summary>
/// 直線状の即時着弾貫通攻撃
/// </summary>
[CreateAssetMenu(fileName = "LaserAttackAction", menuName = "Actions/LaserAttackAction")]
public class LaserAttackAction : ActionBase
{
    [SerializeField, Range(1, 3)] int level = 1;
    [SerializeField] float baseAttackRange = 20;
    [SerializeField] AttackData baseAttackData;

    float Damage => (baseAttackData.damage + _stats.Atk) / interval;

    public override float Evaluate()
    {
        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        return new ActionExecuteInfo(false);
    }
}
