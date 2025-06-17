using UnityEngine;

public class DamageAreaController : MonoBehaviour, ITargetedObject, IAreaObject
{
    [SerializeField] AttackCollider attackCollider;

    public void InitializeTarget(Vector3 targetPosition, AttackData data)
    {
        transform.position = targetPosition;
        attackCollider.data = data;
    }

    public void InitializeArea(float radius)
    {
        transform.localScale = new Vector3(radius, transform.localScale.y, radius);
    }
}
