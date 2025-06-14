using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] HitManager hitManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AttackCollider>(out var attackCollider))
        {
            var attackInfo = new AttackReceiveInfo
            {
                damage = attackCollider.data.damage
            };
            hitManager.ReceiveHit(attackInfo);
        }
    }
}
