using UnityEngine;

public class DamageAreaController : MonoBehaviour, ITargetedObject, IAreaObject
{
    [SerializeField] AttackCollider attackCollider;

    public void InitializeTarget(Vector3 targetPosition, AttackData data)
    {
        transform.position = targetPosition;
        attackCollider.data = data;
    }

    public void InitializeArea(float radius, float lifeTime)
    {
        transform.localScale = new Vector3(radius * 2, transform.localScale.y, radius * 2);
        Destroy(gameObject, lifeTime);
    }
}
