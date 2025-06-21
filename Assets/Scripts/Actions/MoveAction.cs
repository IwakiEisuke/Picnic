using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/MoveAction")]
public class MoveAction : ActionBase
{
    Vector3 _targetPos;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestOpponentAround(transform.position, float.MaxValue, _parent.EntityType, out var target))
        {
            _targetPos = target.transform.position;
            return 0; // 他のアクションを優先する
        }

        return -1;
    }

    public override ActionExecuteInfo Execute()
    {
        _agent.SetDestination(_targetPos);
        return new ActionExecuteInfo(true, this, interval);
    }
}
