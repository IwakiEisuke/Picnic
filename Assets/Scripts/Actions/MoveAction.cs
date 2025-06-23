using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/MoveAction")]
public class MoveAction : ActionBase
{
    [Header("Flee Settings")]
    [SerializeField] bool _flee;
    [SerializeField] float _fleeRange = 3f;
    [SerializeField] float _fleeValueFactor = 1f;
    [SerializeField] float _fleeForceScale = 1f;

    Vector3 _targetPos;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, float.MaxValue, _parent.EntityType, opponent, selfInclude, out var target))
        {
            _targetPos = target.transform.position;

            if (_flee)
            {
                var fleeVector = CalculateFleeVector();
                _targetPos += fleeVector * _fleeForceScale;
                return fleeVector.magnitude * _fleeValueFactor / _fleeRange; // 敵が近ければ逃げるアクションを優先
            }

            return 0; // 他のアクションを優先する
        }

        if (_flee)
        {
            var fleeVector = CalculateFleeVector();
            _targetPos = transform.position + fleeVector * _fleeForceScale;
            return fleeVector.magnitude * _fleeValueFactor / _fleeRange;
        }

        return -1;
    }

    private Vector3 CalculateFleeVector()
    {
        var fleeVector = Vector3.zero;
        var enemies = _parent.Manager.GetEntityAround(_parent, transform.position, _fleeRange, _parent.EntityType, true, false);
        foreach (var enemy in enemies)
        {
            var vec = transform.position - enemy.transform.position;
            var direction = vec.normalized;
            var distance = vec.magnitude;
            fleeVector += (_fleeRange - distance) * direction; // 逃げる方向を計算
        }
        return fleeVector;
    }

    public override ActionExecuteInfo Execute()
    {
        _agent.SetDestination(_targetPos);
        return new ActionExecuteInfo(true, this);
    }
}
