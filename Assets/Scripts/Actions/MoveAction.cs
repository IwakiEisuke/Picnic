using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/MoveAction")]
public class MoveAction : ActionBase
{
    [Range(1, 3)] public int level = 1;

    Transform _target;

    public override float Evaluate()
    {
        var target = FindObjectsByType<UnitBase>(FindObjectsSortMode.None).Where(x => (_parent.opponentLayer.value & 1u << x.gameObject.layer) > 0).OrderBy(x => Vector3.Distance(x.transform.position, _parent.transform.position)).FirstOrDefault();
        if (target != null)
        {
            _target = target.transform;
            return 0; // 他のアクションを優先する
        }

        return -1;
    }

    public override ActionExecuteInfo Execute()
    {
        _agent.SetDestination(_target.position);
        return new ActionExecuteInfo(true, this, interval);
    }
}
