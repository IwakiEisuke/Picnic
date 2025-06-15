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
            // 距離が遠いほど・足が速いほど評価が高い
            float distance = Vector3.Distance(_parent.transform.position, target.transform.position);
            return (distance - _stats.AttackRadius) * level * _stats.Speed;
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
