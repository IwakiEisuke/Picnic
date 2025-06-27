using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] HitManager hitManager;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != gameObject.layer && other.TryGetComponent<AttackCollider>(out var attackCollider))
        {
            if (attackCollider.hitFilter != null && !attackCollider.hitFilter.CanHit(gameObject.transform))
            {
                return;
            }

            var attackInfo = new AttackReceiveInfo(attackCollider.data, other.transform);
            hitManager.ReceiveHit(attackInfo);
        }
    }
}
