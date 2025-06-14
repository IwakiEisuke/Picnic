using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] HitManager hitManager;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<AttackCollider>(out var attackCollider))
        {
            var attackInfo = new AttackReceiveInfo(attackCollider.data, other.transform);
            hitManager.ReceiveHit(attackInfo);
        }
    }
}
