using System.Linq;
using UnityEngine;

[DefaultExecutionOrder((int)ExecutionOrder.AttackCollisionController)]
public class AttackController : MonoBehaviour
{
    [SerializeField] Collider[] attackColliders;

    readonly Collider[] overlapColliders = new Collider[10];

    private void Update()
    {
        for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = false;
        }
    }

    public void SetColliderActive(int i)
    {
        if (i < 0 || i >= attackColliders.Length)
        {
            Debug.LogError($"{name}| SetColliderActive‚Ìˆø”‚Í”ÍˆÍŠO‚Å‚·");
            return;
        }

        attackColliders[i].enabled = true;
    }

    public void AttackNearTarget(Vector3 pos, float radius, AttackData data, Transform attacker, LayerMask layer)
    {
        Physics.OverlapSphereNonAlloc(pos, radius, overlapColliders, layer.value, QueryTriggerInteraction.Collide);
        var target = overlapColliders.Where(x => x != null).OrderBy(x => Vector3.Distance(pos, x.transform.position)).FirstOrDefault();

        if (target != null)
        {
            if (target.attachedRigidbody != null && target.attachedRigidbody.TryGetComponent(out HitManager hitManager) || target.TryGetComponent(out hitManager))
            {
                var attackInfo = new AttackReceiveInfo(data, attacker);
                hitManager.ReceiveHit(attackInfo);
            }
        }
        else
        {
            Debug.LogWarning($"{name}| AttackNearTarget: No valid target found within range.");
        }
    }

    public void AttackDirectly(Transform target, AttackData data)
    {
        if (target.TryGetComponent(out HitManager hitManager))
        {
            hitManager.ReceiveHit(new AttackReceiveInfo(data, transform));
        }
    }
}
