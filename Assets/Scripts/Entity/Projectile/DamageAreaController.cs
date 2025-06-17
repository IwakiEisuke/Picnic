using UnityEngine;

public class DamageAreaController : MonoBehaviour, ITargetedObject
{
    [SerializeField] AttackCollider attackCollider;

    public void InitializeTarget(Vector3 targetPosition, AttackData data)
    {
        transform.position = targetPosition;
        attackCollider.data = data;
    }
}
