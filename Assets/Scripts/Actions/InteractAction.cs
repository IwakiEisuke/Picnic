using System.Linq;
using UnityEngine;

/// <summary>
/// 近いオブジェクトにインタラクトする
/// </summary>
[CreateAssetMenu(fileName = "InteractAction", menuName = "Actions/InteractAction")]
public class InteractAction : ActionBase
{
    [SerializeField] EntityType targetType;

    EntityBase target;

    float InteractRadius => _parent.Status.attackRadius;

    public override float Evaluate()
    {
        if (_parent.Manager.TryGetNearestEntityAround(_parent, transform.position, InteractRadius, targetType, opponent, selfInclude, out target))
        {
            return 1;
        }

        return -1f;
    }

    public override ActionExecuteInfo Execute()
    {
        if (target is IInteractable interactable)
        {
            interactable.Interact();
        }

        return new ActionExecuteInfo(true, this);
    }
}
