using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] HitManager hitManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AttackData>(out var attackData))
        {
            var attackInfo = new AttackReceiveInfo
            {
                damage = attackData.damage
            };
            hitManager.ReceiveHit(attackInfo);
        }
    }
}
