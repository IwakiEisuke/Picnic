using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/MoveAction")]
public class MoveAction : ActionBase
{
    [Range(1, 3)] public int level = 1;

    public override float Evaluate()
    {
        var target = FindObjectsByType<UnitBase>(FindObjectsSortMode.None).Where(x => (x.gameObject.layer & _parent.opponentLayer.value) > 0).OrderBy(x => Vector3.Distance(x.transform.position, _parent.transform.position)).FirstOrDefault();
        if (target != null)
        {
            // ターゲットが存在する場合、評価値を計算
            float distance = Vector3.Distance(_parent.transform.position, target.transform.position);
            if (distance < _stats.AttackRadius)
            {
                // ターゲットが攻撃範囲内にいる場合は移動しない
                return 0f;
            }
            else
            {
                // ターゲットが攻撃範囲外にいる場合は移動する
                return (distance - _stats.AttackRadius) * level * _stats.Speed * Time.deltaTime;
            }
        }

        return level * _stats.Speed * Time.deltaTime;
    }

    public override void Execute()
    {
        var target = FindObjectsByType<UnitBase>(FindObjectsSortMode.None).Where(x => (x.gameObject.layer & _parent.opponentLayer.value) > 0).OrderBy(x => x).FirstOrDefault();
        
        if (target != null)
        {
            _agent.SetDestination(target.transform.position);
        }
        else
        {
            Debug.LogWarning("No target found for MoveAction.");
        }
    }
}
