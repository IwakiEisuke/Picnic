using UnityEngine;

/// <summary>
/// 弾を飛ばす攻撃
/// </summary>
[CreateAssetMenu(fileName = "ProjectileAttackAction", menuName = "Actions/ProjectileAttackAction")]
public class ProjectileAttackAction : ActionBase
{
    public override float Evaluate()
    {
        return 0f;
    }

    public override ActionExecuteInfo Execute()
    {
        return new ActionExecuteInfo(false);
    }
}
