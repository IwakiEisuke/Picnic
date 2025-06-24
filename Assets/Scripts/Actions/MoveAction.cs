using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Actions/MoveAction")]
public class MoveAction : ActionBase
{
    [Header("Move Settings")]
    [SerializeField] bool overwriteTargetType = false;
    [SerializeField] EntityType targetType = EntityType.Object;

    [Header("Flee Settings")]
    [SerializeField] bool _flee;
    [SerializeField] float _fleeRange = 3f;
    [SerializeField] float _fleeValueFactor = 1f;
    [SerializeField] float _fleeForceScale = 1f;

    Vector3 _targetPos;

    EntityType TargetType => overwriteTargetType ? targetType : _parent.EntityType;

    public override float Evaluate()
    {
        // Objectをターゲットにする場合、インタラクト可能な状態にあるオブジェクトへ移動する
        if (TargetType == EntityType.Object)
        {
            var objects = _parent.Manager.GetEntityAround(_parent, transform.position, float.MaxValue, targetType, opponent, selfInclude).OrderBy(obj => (obj.transform.position - transform.position).sqrMagnitude); ;

            foreach (var obj in objects)
            {
                if (obj is IInteractable interactable && interactable.IsInteractable)
                {
                    _targetPos = obj.transform.position;

                    if (_flee)
                    {
                        var fleeVector = CalculateFleeVector();
                        _targetPos += fleeVector * _fleeForceScale;
                        return fleeVector.magnitude * _fleeValueFactor / _fleeRange; // 敵が近ければ逃げるアクションを優先
                    }

                    return 0;
                }
            }
        }

        // 移動先のエンティティを取得
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, float.MaxValue, TargetType, opponent, selfInclude, out var target))
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
