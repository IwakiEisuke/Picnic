using UnityEngine;

/// <summary>
/// 近いオブジェクトにインタラクトする
/// </summary>
[CreateAssetMenu(fileName = "InteractAction", menuName = "Actions/InteractAction")]
public class InteractAction : ActionBase
{
    [SerializeField] EntityType targetType;

    IInteractable _target;

    float InteractRadius => _parent.Status.attackRadius;

    public override float Evaluate()
    {
        var objects = _parent.Manager.GetEntityAround(_parent, transform.position, InteractRadius, targetType, opponent, selfInclude);

        foreach (var obj in objects)
        {
            if (obj is IInteractable interactable && interactable.IsInteractable)
            {
                _target = interactable;
                return 1;
            }
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        if (_target.IsInteractable)
        {
            _target.Interact();
            return new ActionExecuteInfo(true, this);
        }

        return new ActionExecuteInfo(false, this);
    }
}
