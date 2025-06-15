using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] HitManager hitManager;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != gameObject.layer && other.TryGetComponent<AttackCollider>(out var attackCollider))
        {
            var attackInfo = new AttackReceiveInfo(attackCollider.data, other.transform);
            hitManager.ReceiveHit(attackInfo);
        }
    }
}
